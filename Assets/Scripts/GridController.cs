using UnityEngine;
using System.Collections.Generic;

public class GridController {
    private readonly Map map;
    private readonly Block[,] blocksArray;
    private readonly int width;
    private readonly int height;
    
    private readonly Vector3 positionCorrection;

    public GridController(Map map) {
        this.map = map;
        width = map.GetWidth();
        height = map.GetHeight();
        
        blocksArray = map.GetBlockArray();
        
        positionCorrection = map.GetPositionCorrection();
    }

    public void HandleSweptBlocks(List<Block> blocks) {
        DisableSweptOverBlocks(blocks);
        MoveBlocksDown();
        RemoveSweptBlocksFromArray();
    }
    
    private void DisableSweptOverBlocks(List<Block> blocks) {
        int arrayLength = blocks.Count;

        for (int i = 0; i < arrayLength; i++) {
            blocks[i].SetDisabled();
        }
    }
    
    private void MoveBlocksDown() {
        List<Vector2Int> blocksToMoveDown = MoveBlocksDownArrayAndReturnTheirCoordinates();
        MovePhysicallyBlocksDown(blocksToMoveDown);
    }

    private List<Vector2Int> MoveBlocksDownArrayAndReturnTheirCoordinates() {
        List<Vector2Int> blocksToMoveDown = new List<Vector2Int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height - 1; y++) {
                if (!blocksArray[x, y].IsDisabled()) {
                    continue;
                }

                if (IsThereEnabledBlockAbove(x, y)) {
                    MoveEmptyBlockUpArray(x, y);
                    blocksToMoveDown.Add(new Vector2Int(x, y));
                }
            }
        }

        return blocksToMoveDown;
    }

    private bool IsThereEnabledBlockAbove(int x, int y) {
        for (int q = y + 1; q < height; q++) {
            if (blocksArray[x, q].IsDisabled() || blocksArray[x, q].IsUnmovable()) {
                continue;
            }

            return true;
        }

        return false;
    }

    private void MoveEmptyBlockUpArray(int x, int y) {
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

    private void MovePhysicallyBlocksDown(List<Vector2Int> movedBlocks) {
        int movedBLockLength = movedBlocks.Count;

        for (int i = 0; i < movedBLockLength; i++) {
            if (blocksArray[movedBlocks[i].x, movedBlocks[i].y].IsDisabled()) {
                continue;
            }

            blocksArray[movedBlocks[i].x, movedBlocks[i].y]
                    .MoveTo(map.GetWorldPosition(movedBlocks[i].x, movedBlocks[i].y) + positionCorrection);
        }
    }
    
    private void RemoveSweptBlocksFromArray() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (!blocksArray[x, y].IsDisabled()) {
                    continue;
                }

                blocksArray[x, y] = null;
            }
        }
    }
}