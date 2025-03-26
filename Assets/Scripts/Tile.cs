using GeneralScript;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour, IClickEvent {
    [SerializeField]
    private Sprite[] openSprites;
    [SerializeField]
    private SpriteRenderer openSprite;
    [SerializeField]
    private SpriteRenderer blockSprite;
    [SerializeField]
    private Sprite[] flagSprites;
    [SerializeField]
    private SpriteRenderer flagSprite;

    private Board board;
    private Vector2Int gridPosition;
    private bool isOpen;
    private bool isFlag;

    public Vector2Int GridPosition {
        get => gridPosition;
    }

    public bool IsOpen {
        get => isOpen;
    }

    public bool IsFlag {
        get => isFlag;
    }

    public void Initialize(Board board, Vector2Int pos) {
        openSprite.enabled = true;
        openSprite.sprite = null;
        blockSprite.enabled = true;
        flagSprite.enabled = false;

        this.board = board;
        gridPosition = pos;
        isOpen = false;
        isFlag = false;
    }

    public void InitializeOpenTile(int count) {
        openSprite.sprite = openSprites[count];
    }

    public void Open() {
        isOpen = true;
        blockSprite.enabled = false;
        flagSprite.enabled = false;
    }

    public void OnFlag() {

    }

    public void OnMouseLeftButtonDown() {
        if(isOpen) {
            return;
        }

        board.OpenTiles(gridPosition);
    }

    public void OnMouseRightButtonDown() {
        if(isOpen) {
            return;
        }

        isFlag ^= true;
        flagSprite.enabled = isFlag;
    }
}
