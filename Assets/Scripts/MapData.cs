using UnityEngine;

public class MapData {
    private readonly Block[,] blocksArray;
    private readonly int[,] mapData;
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;

    private readonly Vector3 positionCorrection;
    private readonly Vector3 originalPosition;

    public MapData(float cellSize, Vector3 originalPosition, int[,] mapData) {
        this.mapData = mapData;
        width = mapData.GetLength(0);
        height = mapData.GetLength(1);
        this.cellSize = cellSize;
        this.originalPosition = originalPosition;
        positionCorrection = new Vector3(width - cellSize, height - cellSize) * 0.5f;
        blocksArray = new Block[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originalPosition - positionCorrection;
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

    public int[,] GetMapData() {
        return mapData;
    }
}