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
        boardHeight = height;
        boardWidth = width;
        tileSize = size;
        mineCount = mine;
        closeTileCount = boardHeight * boardWidth;
        flagTileCount = 0;

        CreateTileObject();
    }

    public void OpenTiles(Vector2Int gridPos) {
        // 初回オープンされた際にタイル情報をセット⇒一手目は安全
        if(tileStates == null) {
            DebugLogger.Log("Initialize OpenTiles");
            SetTileOpenState(gridPos);
        }

        if(gameManager.IsGameOver) {
            return;
        }

        OpenAroundTile(gridPos);
    }

    public void FlagTiles(Vector2Int gridPos) {

    }

    private void OpenAroundTile(Vector2Int gridPos) {
        if(!IsWithinGrid(gridPos)) {
            return;
        }

        // すでに開かれているもしくはフラグが立っているならスキップ
        if(tileObjects[gridPos.y, gridPos.x].IsOpen || tileObjects[gridPos.y, gridPos.x].IsFlag) {
            return;
        }

        DebugLogger.Log(gridPos);
        tileObjects[gridPos.y, gridPos.x].Open();
        closeTileCount--;

        // 地雷に隣接もしくは地雷マスであれば周りは開かない
        if(tileStates[gridPos.y, gridPos.x] != TileState.EmptyId) {
            // 地雷だったらゲームオーバー
            if(tileStates[gridPos.y, gridPos.x] == TileState.MineId) {
                gameManager.GameOver();
            }
            return;
        }

        foreach(Vector2Int offset in offsets) {
            OpenAroundTile(gridPos + offset);
        }
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

    private void SetTileOpenState(Vector2Int clickPos) {
        tileStates = new int[boardHeight, boardWidth];  // 0初期化済み

        foreach(Vector2Int minePos in GenerateBuryingMinesPosition()) {
            tileStates[minePos.y, minePos.x] = TileState.MineId;

            foreach(Vector2Int offset in offsets) {
                Vector2Int pos = minePos - offset;

                if(!IsWithinGrid(pos)) {
                    continue;
                }

                //// 地雷ならスキップ
                if(tileStates[pos.y, pos.x] == TileState.MineId) {
                    continue;
                }

                tileStates[pos.y, pos.x]++;
            }
        }

        for(int y = 0; y < boardHeight; y++) {
            for(int x = 0; x < boardWidth; x++) {
                tileObjects[y, x].InitializeOpenTile(tileStates[y, x]);
            }
        }
    }

    private Vector2Int[] GenerateBuryingMinesPosition() {
        List<int> randList = UniqueRandomGenerator.UniqueRandomInt(0, boardWidth * boardHeight, mineCount);
        Vector2Int[] minePositions = new Vector2Int[mineCount];

        for(int i = 0; i < randList.Count; i++) {
            minePositions[i] = new Vector2Int(randList[i] % boardWidth, randList[i] / boardWidth);
        }

        return minePositions;
    }

    private bool IsWithinGrid(Vector2Int pos) {
        if(pos.y < 0 || pos.y >= boardHeight || pos.x < 0 || pos.x >= boardWidth) {
            return false;
        }

        return true;
    }

    private void TileFullOpen() {
        foreach(Tile tile in tileObjects) {
            if(tile.IsOpen) {
                continue;
            }

            tile.Open();
        }
    }
}
