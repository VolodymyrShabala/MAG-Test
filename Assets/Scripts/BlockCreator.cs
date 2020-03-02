using System.Collections.Generic;
using UnityEngine;

public class BlockCreator {
    private readonly MapData mapData;
    private readonly Transform parent;
    
    public BlockCreator(MapData mapData, Transform parent) {
        this.mapData = mapData;
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
        Vector3 gridPosition = mapData.GetWorldPosition(x, y) + mapData.GetPositionCorrection();
        Vector3 spawnPosition = mapData.GetWorldPosition(x, mapData.GetHeight()) + mapData.GetPositionCorrection() + Vector3.up * mapData.GetSpawnOffset();

        if (!block) {
            block = Object.Instantiate(mapData.GetBlockPrefab(), parent);
        }

        block.SetEnabled();
        block.transform.position = spawnPosition;
        block.MoveTo(gridPosition);
        block.SetBlockType((BlockType) blockType);
        mapData.SetNewBlock(block, x, y);
    }

    
}