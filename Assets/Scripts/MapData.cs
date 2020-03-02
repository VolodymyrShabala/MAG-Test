using System.Collections.Generic;
using UnityEngine;

public class MapData {
    private readonly Block blockPrefab;
    private readonly Block[,] blocksArray;
    private readonly int[,] mapData;
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly float spawnOffset;

    private readonly Vector3 positionCorrection;
    private readonly Vector3 originalPosition;

    public MapData(Block blockPrefab, float cellSize, float spawnOffset, Vector3 originalPosition, int[,] mapData) {
        this.mapData = mapData;
        width = mapData.GetLength(0);
        height = mapData.GetLength(1);
        this.blockPrefab = blockPrefab;
        this.cellSize = cellSize;
        this.spawnOffset = spawnOffset;
        this.originalPosition = originalPosition;
        positionCorrection = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        blocksArray = new Block[width, height];
    }

    public void RemoveBlocksFromArray(List<Block> blocksToRemove) {
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

    public float GetSpawnOffset() {
        return spawnOffset;
    }

    public Vector3 GetPositionCorrection() {
        return positionCorrection;
    }

    public int[,] GetMapData() {
        return mapData;
    }

    public Block GetBlockPrefab() {
        return blockPrefab;
    }
}