using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour{
    private Block[,] blocksArray;
    private Grid grid;

    private int width;
    private int height;
    private float cellSize;
    private float spawnOffset;
    private Vector3 positionCorrection;
    private Block[] blocksToReturn;

    public void Init(int width, int height, float cellSize, Vector3 gridPosition, float spawnOffset){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.spawnOffset = spawnOffset;
        positionCorrection = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        gridPosition -= new Vector3(width * 0.5f, height * 0.5f);
        grid = new Grid(width, height, cellSize, gridPosition);

        blocksArray = new Block[width, height];
        PopulateBoard();
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
            Unselect(blocks[i]);
        }
    }

    public BlockColor GetBlockColor(Vector2Int block){
        return blocksArray[block.x, block.y].GetBlockColor();
    }

    public bool CanBeSelected(Vector2Int block, BlockColor blockColor){
        return blocksArray[block.x, block.y].CanBeSelected(blockColor);
    }

    public void DeleteBlocksAll(Vector2Int[] blocks){
        int arrayLength = blocks.Length;
        for(int i = 0; i < arrayLength; i++) {
            blocksArray[blocks[i].x, blocks[i].y].SetDisabled();
            // print($"Block {blocks[i].x}|{blocks[i].y} is disabled");
        }
    }

    // TODO: Sometimes two enabled switch places
    public void MoveBlocksDown(Vector2Int[] deletedBlocks){
        blocksToReturn = new Block[deletedBlocks.Length];
        print(deletedBlocks.Length);
        foreach(Vector2Int block in deletedBlocks) {
            int x = block.x;
            for(int y = 0; y < height - 1; y++) {
                if(!blocksArray[x, y].IsDisabled()) {
                    continue;
                }

                for(int q = y + 1; q < height; q++) {
                    if(!blocksArray[x, q].IsDisabled()) {
                        // print($"Swapping place of {x}|{y} is disabled with {x}|{q}");
                        Block tempBlock = blocksArray[x, y];
                        blocksArray[x, y] = blocksArray[x, q];
                        blocksArray[x, q] = tempBlock;
                        // print($"Block {x}|{q} is disabled: {blocksArray[x, q].IsDisabled()}");
                        break;
                    }
                }

                // SwapBlocks(x, y);
            }
        }

        int index = 0;
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(!blocksArray[x, y].IsDisabled()) {
                    blocksArray[x, y].MoveTo(grid.GetWorldPosition(x, y) + positionCorrection);
                    continue;
                }

                print(index);
                blocksToReturn[index] = blocksArray[x, y];
                index++;
            }
        }
        
        print(index);
    }

    private void SwapBlocks(int x, int y){
        for(int q = y + 1; q < height; q++) {
            if(!blocksArray[x, q].IsDisabled()) {
                Block tempBlock = blocksArray[x, y];
                blocksArray[x, y] = blocksArray[x, q];
                blocksArray[x, q] = tempBlock;
                break;
            }
        }
    }

    public void ReturnBlocksToPool(){
        int blocksToReturnLength = blocksToReturn.Length;
        for(int i = 0; i < blocksToReturnLength; i++) {
            blocksToReturn[i].ReturnToPool();
        }

        blocksToReturn = new Block[0];
    }

    public void RepopulateBoard(Vector2Int[] deletedBlocks){
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                // print($"Is disabled {x}|{y}: {blocksArray[x, y].IsDisabled()}");
                if(blocksArray[x, y].IsDisabled()) {
                    // print($"Spawning {x}|{y}");
                    Vector3 position = grid.GetWorldPosition(x, y) + positionCorrection;
                    SpawnBlock(position, x, y);
                }
            }
        }
    }

    private void PopulateBoard(){
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                SpawnBlock(grid.GetWorldPosition(x, y) + positionCorrection, x, y);
            }
        }
    }

    private void SpawnBlock(Vector3 position, int x, int y){
        int randomBlock = Random.Range(0, (int) BlockColor.MAX);
        blocksArray[x, y] = ObjectPool.GetInstance.GetBlock((BlockColor) randomBlock);
        blocksArray[x, y].transform.position = position + Vector3.up * spawnOffset;
        blocksArray[x, y].SetEnabled();
        blocksArray[x, y].MoveTo(position);
    }

    public bool IsAdjacentTo(Vector2Int block, Vector2Int neighbourBlock){
        return grid.IsAdjacentTo(block, neighbourBlock);
    }

    public Vector2Int GetXY(Vector3 worldPosition){
        return grid.GetXY(worldPosition);
    }

    public bool IsWithingGrid(Vector2Int coordinates){
        return grid.IsWithingGrid(coordinates);
    }
}