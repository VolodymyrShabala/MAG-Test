using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour{
    private Block[,] blocksArray;

    private int width;
    private int height;
    private float cellSize;
    private float spawnOffset;

    public void Init(int width, int height, float cellSize){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        blocksArray = new Block[width, height];
    }

    public void Select(Vector2Int block){
        blocksArray[block.x, block.y].Select();
    }

    public void Unselect(Vector2Int block){
        blocksArray[block.x, block.y].UnSelect();
    }

    public void UnselectAll(Vector2Int[] blocks){
        int arrayLength = blocks.Length;
        for(int i = 0; i < arrayLength; i++) {
            blocksArray[blocks[i].x, blocks[i].y].UnSelect();
        }
    }

    public BlockColor GetBlockColor(Vector2Int block){
        return blocksArray[block.x, block.y].GetBlockColor();
    }

    public bool CanBeSelected(Vector2Int block, BlockColor blockColor){
        return blocksArray[block.x, block.y].CanBeSwipedOver(blockColor);
    }

    public void DeleteBlock(Vector2Int block){
        blocksArray[block.x, block.y].Reset();
        ObjectPool.GetInstance.ReturnToPool(blocksArray[block.x, block.y]);
        blocksArray[block.x, block.y] = null;
    }

    public void DeleteBlockAll(Vector2Int[] blocks){
        int arrayLength = blocks.Length;
        for(int i = 0; i < arrayLength; i++) {
            blocksArray[blocks[i].x, blocks[i].y].Reset();
            blocksArray[blocks[i].x, blocks[i].y].SetDisabled();
            ObjectPool.GetInstance.ReturnToPool(blocksArray[blocks[i].x, blocks[i].y]);
        }
    }

    // TODO: Again overlapping
    public void MoveBlocksDown(Vector2Int[] deletedBlocks){
        foreach(Vector2Int block in deletedBlocks) {
            int x = block.x;
            for(int y = block.y; y < height; y++) {
                if(y >= height - 1) {
                    break;
                }

                Block tempBlock = blocksArray[x, y];
                blocksArray[x, y] = blocksArray[x, y + 1];
                blocksArray[x, y + 1] = tempBlock;
                blocksArray[x, y].MoveDownOneUnit();
            }
        }
    }

    // TODO: Request position
    public void RepopulateBoard(Vector2Int[] deletedBlocks){
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(blocksArray[x, y].Disabled()) {
                    SpawnBlock(blocksArray[x, y].transform.position, x, y);
                }
            }
        }
        
        // for(int x = 0; x < width; x++) {
        //     for(int y = 0; y < height; y++) {
        //         Vector3 truePosition = grid.GetWorldPosition(x, y) + new Vector3(halfCell, halfCell);
        //         if(blocksArray[x, y] != null) {
        //             blocksArray[x, y].MoveTo(truePosition);
        //         } else {
        //             int randomBlock = Random.Range(1, (int)BlockColor.MAX);
        //             blocksArray[x, y] = ObjectPool.GetInstance.GetBlock((BlockColor) randomBlock);
        //             blocksArray[x, y].SetInUse();
        //             blocksArray[x, y].transform.position = truePosition + new Vector3(0, spawnOffset);
        //             blocksArray[x, y].MoveTo(grid.GetWorldPosition(x, y) + new Vector3(halfCell, halfCell));
        //
        //         }
        //     }
        // }
    }

    public void SpawnBlock(Vector3 position, int x, int y){
        int randomBlock = Random.Range(0, (int) BlockColor.MAX);
        blocksArray[x, y] = ObjectPool.GetInstance.GetBlock((BlockColor) randomBlock);
        blocksArray[x, y].transform.position = position + new Vector3(0, spawnOffset);
        blocksArray[x, y].MoveTo(position);
    }
}