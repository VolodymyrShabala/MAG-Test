using System.Collections.Generic;
using UnityEngine;

public class BlockCreator {
    private readonly MapData mapData;
    private readonly float spawnOffset;
    private readonly Block blockPrefab;
    private readonly Transform parent;
    
    public BlockCreator(MapData mapData, float spawnOffset, Block blockPrefab, Transform parent) {
        this.mapData = mapData;
        this.spawnOffset = spawnOffset;
        this.blockPrefab = blockPrefab;
        this.parent = parent;

        PopulateBoard(mapData.GetMapData());
    }

    private void PopulateBoard(int[,] levelData) {
        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight(); y++) {
                SpawnBlock(levelData[x, y], x, y);
            }
        }
    }

    public void RepopulateBoard(List<Block> blocksToUse) {
        int index = 0;
        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight(); y++) {
                if (mapData.GetBlock(x, y)) {
                    continue;
                }

                SpawnBlock(Random.Range(0, (int) BlockType.DestructibleObstacle), x, y, blocksToUse[index++]);
            }
        }
    }

    private void SpawnBlock(int blockType, int x, int y, Block block = null) {
        Vector3 gridPosition = mapData.GetWorldPosition(x, y);
        Vector3 spawnPosition = mapData.GetWorldPosition(x, mapData.GetHeight()) + Vector3.up * spawnOffset;

        if (!block) {
            block = Object.Instantiate(blockPrefab, parent);
            block.name = $"Block {x}|{y}";
        }

        block.SetEnabled();
        block.transform.position = spawnPosition;
        block.MoveTo(gridPosition);
        block.SetBlockType((BlockType) blockType);
        mapData.SetNewBlock(block, x, y);
    }
}