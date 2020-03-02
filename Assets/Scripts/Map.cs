using UnityEngine;

public class Map {
    private readonly Block[,] blocksArray;
    private readonly int[,] mapData;
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;

    private readonly Vector3 positionCorrection;
    private readonly Vector3 originalPosition;

    public Map(TextAsset levelFile, int defaultWidth, int defaultHeight, float cellSize, Vector3 originalPosition) {
        if (levelFile) {
            mapData = FileReader.ReadLevel(levelFile);
            width = mapData.GetLength(0);
            height = mapData.GetLength(1);
        } else {
            width = defaultWidth;
            height = defaultHeight;
        }

        this.cellSize = cellSize;
        this.originalPosition = originalPosition;
        positionCorrection = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        
        blocksArray = new Block[width, height];
    }
    
    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originalPosition - new Vector3(width * 0.5f, height * 0.5f);
    }
    
    public void SetNewBlock(Block block, int x, int y) {
        blocksArray[x, y] = block;
    }

    public Block GetBlock(int x, int y) {
        return blocksArray[x, y];
    }
    
    public int GetWidth() {
        return width;
    }
    
    public int GetHeight() {
        return height;
    }

    public Vector3 GetPositionCorrection() {
        return positionCorrection;
    }

    public int[,] GetMapData() {
        return mapData;
    }
}