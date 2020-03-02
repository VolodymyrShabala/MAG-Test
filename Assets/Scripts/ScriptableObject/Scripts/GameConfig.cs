using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Game Configuration")]
public class GameConfig : ScriptableObject {
    [SerializeField] private TextAsset levelFileTXT;
    [SerializeField] private Block blockPrefab;
    
    [SerializeField] private float spawnOffset = 1;
    [SerializeField] private float cellSize = 1;
    [Tooltip("How many blocks are needed to be selected for them to be counted")]
    [SerializeField] private int amountOfSelectedBlocksToDestroy = 3;
    
    [Header("Used if no level file was assigned")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    
    public int GetWidth() {
        return width;
    }
    
    public int GetHeight() {
        return height;
    }

    public int GetAmountOfSelectedBlocksToDestroy() {
        return amountOfSelectedBlocksToDestroy;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public float GetSpawnOffset() {
        return spawnOffset;
    }

    public Block GetBlockPrefab() {
        return blockPrefab;
    }

    public TextAsset GetLevelFile() {
        return levelFileTXT;
    }
}