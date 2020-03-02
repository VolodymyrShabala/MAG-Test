using System.Collections.Generic;
using UnityEngine;

public class BlockCreator {
    private readonly Map map;
    private readonly Block blockPrefab;
    private readonly Transform parent;

    private readonly float spawnOffset;
    
    public BlockCreator(Map map, Block blockPrefab, Transform parent, float spawnOffset) {
        this.map = map;
        this.parent = parent;
        this.blockPrefab = blockPrefab;
        this.spawnOffset = spawnOffset;
        
        PopulateBoard(map.GetMapData());
    }

    private void PopulateBoard(int[,] levelData) {
        bool levelLoaded = levelData != null;
        for (int x = 0; x < map.GetWidth(); x++) {
            for (int y = 0; y < map.GetHeight(); y++) {
                SpawnBlock(levelLoaded? levelData[x, y] : Random.Range(0, (int) BlockType.DestructibleObstacle), x, y);
            }
        }
    }

    public void RepopulateBoard(List<Block> blocksToUse) {
        for (int x = 0; x < map.GetWidth(); x++) {
            for (int y = 0; y < map.GetHeight(); y++) {
                if (map.GetBlockArray()[x, y]) {
                    continue;
                }

                SpawnBlock(Random.Range(0, (int) BlockType.DestructibleObstacle), x, y, blocksToUse[0]);
                blocksToUse.RemoveAt(0);
            }
        }
    }

    private void SpawnBlock(int blockType, int x, int y, Block block = null) {
        Vector3 gridPosition = map.GetWorldPosition(x, y) + map.GetPositionCorrection();
        Vector3 spawnPosition = map.GetWorldPosition(x, map.GetHeight()) + map.GetPositionCorrection() + Vector3.up * spawnOffset;

        if (!block) {
            block = GameObject.Instantiate(blockPrefab, parent);
        }

        block.SetEnabled();
        block.transform.position = spawnPosition;
        block.MoveTo(gridPosition);
        block.SetBlockType((BlockType) blockType);
        map.GetBlockArray()[x, y] = block;
    }

    
}