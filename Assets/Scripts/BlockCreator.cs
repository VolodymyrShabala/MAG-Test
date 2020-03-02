using System.Collections.Generic;
using UnityEngine;

public class BlockCreator {
    private readonly Map map;
    private readonly Block blockPrefab;
    private readonly Transform parent;

    private readonly int width;
    private readonly int height;
    private readonly float spawnOffset;
    private readonly Block[,] blocksArray;
    private readonly Vector3 positionCorrection;
    
    public BlockCreator(Map map, Block blockPrefab, Transform parent, float spawnOffset) {
        this.map = map;
        width = map.GetWidth();
        height = map.GetHeight();
        this.parent = parent;
        this.blockPrefab = blockPrefab;
        blocksArray = map.GetBlockArray();
        this.spawnOffset = spawnOffset;
        
        positionCorrection = map.GetPositionCorrection();
        
        PopulateBoard(map.GetMapData());
    }

    private void PopulateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                SpawnBlock(Random.Range(0, (int) BlockType.MAX), x, y);
            }
        }
    }
    
    private void PopulateBoard(int[,] levelData) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                SpawnBlock(levelData[x, y], x, y);
            }
        }
    }

    public void RepopulateBoard(List<Block> blocksToUse) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (blocksArray[x, y]) {
                    continue;
                }

                SpawnBlock(Random.Range(0, (int) BlockType.DestructibleObstacle), x, y, blocksToUse[0]);
                blocksToUse.RemoveAt(0);
            }
        }
    }

    private void SpawnBlock(int blockType, int x, int y, Block block = null) {
        Vector3 gridPosition = map.GetWorldPosition(x, y) + positionCorrection;
        Vector3 spawnPosition = map.GetWorldPosition(x, height) + positionCorrection + Vector3.up * spawnOffset;

        if (!block) {
            block = GameObject.Instantiate(blockPrefab, parent);
        }

        block.SetEnabled();
        block.transform.position = spawnPosition;
        block.MoveTo(gridPosition);
        block.SetBlockType((BlockType) blockType);
        blocksArray[x, y] = block;
    }

    
}