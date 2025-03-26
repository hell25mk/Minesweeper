using GeneralScript;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField]
    private GameObject tilePrefab;

    private GameManager gameManager;
    private readonly Vector2Int[] offsets = new Vector2Int[] {
        new Vector2Int( 1, -1),
        new Vector2Int( 1,  0),
        new Vector2Int( 1,  1),
        new Vector2Int( 0, -1),
        new Vector2Int( 0,  1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1,  0),
        new Vector2Int(-1,  1),
    };

    private bool isGameStart;
    private int boardWidth;
    private int boardHeight;
    private float tileSize;
    private int mineCount;
    private Tile[,] tileObjects;
    private int[,] tileStates;

    private int closeTileCount;
    private int flagTileCount;

    public void Initialize(GameManager gm, int height, int width, float size, int mine) {
        gameManager = gm;
        isGameStart = false;
        boardHeight = height;
        boardWidth = width;
        tileSize = size;
        mineCount = mine;
        closeTileCount = boardHeight * boardWidth;
        flagTileCount = 0;

        CreateTileObject();
    }

    public void OpenTile(Vector2Int gridPos) {
        if(!IsWithinBoard(gridPos)) {
            return;
        }

        // すでに開かれているもしくはフラグが立っているならスキップ
        if(tileObjects[gridPos.y, gridPos.x].IsOpen || tileObjects[gridPos.y, gridPos.x].IsFlag) {
            return;
        }

        // 初回オープンされた際にタイル情報をセット⇒一手目は安全
        if(!isGameStart) {
            DebugLogger.Log("Initialize OpenTiles");
            InitializeTileState(gridPos);
            isGameStart = true;
        }

        // ゲームオーバー状態なら処理を終了する
        // TODO: GameManager側で制御するほうがいいかも
        if(gameManager.IsGameOver) {
            return;
        }

        DebugLogger.Log(gridPos);
        TileOpenBFS(gridPos);
        //tileObjects[gridPos.y, gridPos.x].Open();
        //closeTileCount--;

        //// 地雷に隣接もしくは地雷マスであれば周りは開かない
        //if(tileObjects[gridPos.y, gridPos.x].BuriedItem != BuriedItemType.Empty) {
        //    // 地雷だったらゲームオーバー
        //    if(tileObjects[gridPos.y, gridPos.x].BuriedItem == BuriedItemType.Mine) {
        //        gameManager.GameOver();
        //    }
        //    return;
        //}

        //foreach(Vector2Int offset in offsets) {
        //    OpenTile(gridPos + offset);
        //}
    }

    public void FlagTiles(Vector2Int gridPos) {
        flagTileCount++;
    }

    private void CreateTileObject() {
        tileObjects = new Tile[boardHeight, boardWidth];

        for(int y = 0; y < boardHeight; y++) {
            for(int x = 0; x < boardWidth; x++) {
                Vector2 pos = new Vector3(
                    transform.position.x + x * tileSize,
                    transform.position.y + y * tileSize
                 );

                GameObject tileObject = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileObject.transform.parent = transform;
                Tile tile = tileObject.GetComponent<Tile>();
                tile.Initialize(this, new Vector2Int(x, y));
                tileObjects[y, x] = tile;
            }
        }
    }

    private void InitializeTileState(Vector2Int gridPos) {
        // TODO: gridPosは地雷にならないような処理を追加する
        BuriedItemType[,] tileStates = new BuriedItemType[boardHeight, boardWidth];

        foreach(Vector2Int minePos in GenerateBuryingMinesPosition(gridPos)) {
            tileStates[minePos.y, minePos.x] = BuriedItemType.Mine;

            foreach(Vector2Int offset in offsets) {
                Vector2Int pos = minePos - offset;

                if(!IsWithinBoard(pos)) {
                    continue;
                }

                //// 地雷ならスキップ
                if(tileStates[pos.y, pos.x] == BuriedItemType.Mine) {
                    continue;
                }

                tileStates[pos.y, pos.x] = tileStates[pos.y, pos.x].Next();
            }
        }

        for(int y = 0; y < boardHeight; y++) {
            for(int x = 0; x < boardWidth; x++) {
                tileObjects[y, x].InitializeBuriedItem(tileStates[y, x]);
            }
        }
    }

    private Vector2Int[] GenerateBuryingMinesPosition(Vector2Int excludePos) {
        List<int> excludes = new List<int>{ (excludePos.x + excludePos.y * boardWidth) };
        List<int> randList = UniqueRandomGenerator.UniqueRandomInt(0, boardWidth * boardHeight, mineCount, excludes);
        Vector2Int[] minePositions = new Vector2Int[mineCount];

        for(int i = 0; i < randList.Count; i++) {
            minePositions[i] = new Vector2Int(randList[i] % boardWidth, randList[i] / boardWidth);
        }

        return minePositions;
    }

    private bool IsWithinBoard(Vector2Int pos) {
        if(pos.y < 0 || pos.y >= boardHeight || pos.x < 0 || pos.x >= boardWidth) {
            return false;
        }

        return true;
    }

    // BreadthFirstSearch
    private void TileOpenBFS(Vector2Int first) {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[boardHeight, boardWidth];

        queue.Enqueue(first);
        visited[first.y, first.x] = true;

        while(queue.Count > 0) {
            Vector2Int current = queue.Dequeue();

            Tile tile = tileObjects[current.y, current.x];

            if(tile.IsOpen || tile.IsFlag) {
                continue;
            }

            tile.Open();

            if(tile.BuriedItem != BuriedItemType.Empty) {
                if(tile.BuriedItem == BuriedItemType.Mine) {
                    gameManager.GameOver();
                }
                continue;
            }

            foreach(Vector2Int offset in offsets) {
                Vector2Int next = current + offset;

                if(!IsWithinBoard(next) || visited[next.y, next.x]) {
                    continue;
                }

                queue.Enqueue(next);
                visited[next.y, next.x] = true;
            }
        }
    }

    public void TileFullOpen() {
        foreach(Tile tile in tileObjects) {
            if(tile.IsOpen) {
                continue;
            }

            tile.Open();
        }
    }
}
