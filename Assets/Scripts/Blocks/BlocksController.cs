using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksController {
    private readonly Block[,] blocksArray;

    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly float spawnOffset;
    private readonly Vector3 positionCorrection;
    private readonly Vector3 originalPosition;

    public BlocksController(int width, int height, float cellSize, Vector3 originalPosition, float spawnOffset) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.spawnOffset = spawnOffset;
        this.originalPosition = originalPosition;
        positionCorrection = new Vector3(cellSize * 0.5f, cellSize * 0.5f);

        blocksArray = new Block[width, height];
        PopulateBoard();
    }

    public void AddBomb(int x, int y) {
        blocksArray[x, y].gameObject.AddComponent<Bomb>();
    }

    public void AddBombRandom() {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);

        if (!blocksArray[x, y].IsDisabled()) {
            blocksArray[x, y].gameObject.AddComponent<Bomb>();
        }
    }

    public void Select(Vector2Int block) {
        blocksArray[block.x, block.y].Select();
    }

    public void Unselect(Vector2Int block) {
        blocksArray[block.x, block.y].UnSelect();
    }

    public void UnselectAll(Vector2Int[] blocks) {
        int arrayLength = blocks.Length;

        for (int i = 0; i < arrayLength; i++) {
            Unselect(blocks[i]);
        }
    }

    public void SweepEnd(Vector2Int[] blocks) {
        DisableSweptOverBlocks(blocks);
        MoveBlocksDown();
        RepopulateBoard();
    }

    private void DisableSweptOverBlocks(Vector2Int[] blocks) {
        int arrayLength = blocks.Length;

        for (int i = 0; i < arrayLength; i++) {
            blocksArray[blocks[i].x, blocks[i].y].SetDisabled();
            CheckBomb(blocks[i].x, blocks[i].y);
        }
    }

    private void CheckBomb(int x, int y) {
        if (blocksArray[x, y].blockType != BlockType.Bomb) {
            return;
        }

        BombDirection bombDirection = blocksArray[x, y].GetComponentInChildren<Bomb>().bombDirection;
        List<Vector2Int> bombDestroy = new List<Vector2Int>();

        switch (bombDirection) {
            case BombDirection.Vertical: {
                for (int i = 0; i < height; i++) {
                    if (!blocksArray[x, i].IsDisabled() || blocksArray[x, i].IsUnmovable()) {
                        bombDestroy.Add(new Vector2Int(x, i));
                    }
                }

                break;
            }

            case BombDirection.Horizontal: {
                for (int i = 0; i < width; i++) {
                    if (!blocksArray[i, y].IsDisabled() || blocksArray[i, y].IsUnmovable()) {
                        bombDestroy.Add(new Vector2Int(i, y));
                    }
                }

                break;
            }
        }

        DisableSweptOverBlocks(bombDestroy.ToArray());
    }

    private void MoveBlocksDown() {
        List<Vector2Int> movedBlocks = MoveBlocksDownArray();
        MoveBlocksDownObject(movedBlocks);
    }

    private List<Vector2Int> MoveBlocksDownArray() {
        List<Vector2Int> movedBlocks = new List<Vector2Int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height - 1; y++) {
                if (!blocksArray[x, y].IsDisabled()) {
                    continue;
                }

                SwapBlocks(x, y);
                movedBlocks.Add(new Vector2Int(x, y));
            }
        }

        return movedBlocks;
    }

    private void MoveBlocksDownObject(List<Vector2Int> movedBlocks) {
        int movedBLockLength = movedBlocks.Count;

        for (int i = 0; i < movedBLockLength; i++) {
            if (blocksArray[movedBlocks[i].x, movedBlocks[i].y].IsDisabled()) {
                continue;
            }

            blocksArray[movedBlocks[i].x, movedBlocks[i].y]
                    .MoveTo(GetWorldPosition(movedBlocks[i].x, movedBlocks[i].y) + positionCorrection);
        }
    }

    private void SwapBlocks(int x, int y) {
        for (int q = y + 1; q < height; q++) {
            if (blocksArray[x, q].IsDisabled() || blocksArray[x, q].IsUnmovable()) {
                continue;
            }

            Block tempBlock = blocksArray[x, y];
            blocksArray[x, y] = blocksArray[x, q];
            blocksArray[x, q] = tempBlock;
            break;
        }
    }

    private void ReturnBlocksToPool(int x, int y) {
        ObjectPool.GetInstance.ReturnToPool(blocksArray[x, y]);
    }

    private void RepopulateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (!blocksArray[x, y].IsDisabled() || blocksArray[x, y].IsUnmovable()) {
                    continue;
                }

                Vector3 position = GetWorldPosition(x, y) + positionCorrection;
                ReturnBlocksToPool(x, y);
                SpawnBlock(position, x, y);
            }
        }
    }

    private void PopulateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                SpawnBlock(GetWorldPosition(x, y) + positionCorrection, x, y);
            }
        }
    }

    private void SpawnBlock(Vector3 position, int x, int y) {
        int randomBlock = Random.Range(0, (int) BlockColor.MAX);
        Vector3 spawnPosition = GetWorldPosition(x, height) + positionCorrection + Vector3.up * spawnOffset;

        blocksArray[x, y] = ObjectPool.GetInstance.GetBlock((BlockColor) randomBlock);
        blocksArray[x, y].transform.position = spawnPosition;
        blocksArray[x, y].SetEnabled();
        blocksArray[x, y].MoveTo(position);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originalPosition;
    }

    public BlockColor GetBlockColor(Vector2Int block) {
        return blocksArray[block.x, block.y].GetBlockColor();
    }

    public bool CanBeSelected(Vector2Int block, BlockColor blockColor) {
        return blocksArray[block.x, block.y].CanBeSelected(blockColor);
    }
}