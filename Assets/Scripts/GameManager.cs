using GeneralScript;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private BoardConfig boardConfig;
    [SerializeField]
    private GameObject boardPrefab;

    private MouseManager mouseManager;
    private bool isGameOver;
    private Board board;
    private int boardHeight;
    private int boardWidth;
    private float tileSize;
    private int mineCount;

    public bool IsGameOver {
        get => isGameOver;
    }

    public void Start() {
        mouseManager = GetComponent<MouseManager>();
        isGameOver = false;
        boardHeight = boardConfig.boardHeight;
        boardWidth = boardConfig.boardWidth;
        tileSize = boardConfig.tileSize;
        mineCount = boardConfig.mineCount;

        InitializeBoard();
    }

    public void Update() {
        mouseManager.OnUpdate();
    }

    private void InitializeBoard() {
        // ÉJÉÅÉâÇ…çáÇÌÇπÇƒà íuí≤êÆ(óvåüì¢)
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 boardPos = new Vector3(
            cameraPos.x - boardWidth / 2 + tileSize / 2.0f,
            cameraPos.y - boardHeight / 2 + tileSize / 2.0f
        );
        GameObject boardObject = Instantiate(boardPrefab, boardPos, Quaternion.identity);
        board = boardObject.GetComponent<Board>();
        board.Initialize(this, boardHeight, boardWidth, tileSize, mineCount);
    }

    public void GameOver() {
        isGameOver = true;
        GeneralScript.DebugLogger.Log("GameOver");
    }
}
