using UnityEngine;

[ExecuteInEditMode]
public class GridBuilder : MonoBehaviour {
    public BlockType blockType;
    public TextAsset levelFileTXT;
    
    [Header("Grid")]
    public int width;
    public int height;
    public float cellSize;

    public int[,] levelGrid;

    private void OnEnable() {
        levelGrid = new int[width, height];
    }

    public void LoadLevel() {
        levelGrid = FileReader.ReadLevel(levelFileTXT);
    }
    
    public void SaveLevel() {
        FileReader.WriteLevel(levelFileTXT, levelGrid);
    }
}