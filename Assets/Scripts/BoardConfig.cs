using UnityEngine;

[CreateAssetMenu(menuName = "Config/BoardConfig")]
public class BoardConfig : ScriptableObject {
    public int boardWidth = 10;
    public int boardHeight = 10;
    public float tileSize = 1.0f;
    public int mineCount = 20;
}
