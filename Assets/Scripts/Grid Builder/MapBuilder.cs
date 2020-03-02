using UnityEngine;

[ExecuteInEditMode]
public class MapBuilder : MonoBehaviour {
    public BlockType blockType;
    public TextAsset levelFileTXT;
    
    [Header("Grid")]
    public int width = 5;
    public int height = 5;
    public float cellSize = 2;

    public int[,] levelGrid;

    private void OnEnable() {
        levelGrid = new int[width, height];
    }

    public void LoadLevel() {
        levelGrid = LevelReader.ReadLevel(levelFileTXT);
    }
    
    public void SaveLevel() {
        LevelReader.WriteLevel(levelFileTXT, levelGrid);
    }
}