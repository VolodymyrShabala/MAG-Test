using UnityEngine;
using System.Collections.Generic;

public class GridController {
    private readonly Map map;

    public GridController(Map map) {
        this.map = map;
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

        for (int x = 0; x < map.GetWidth(); x++) {
            for (int y = 0; y < map.GetHeight() - 1; y++) {
                if (!map.GetBlock(x, y).IsDisabled()) {
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
        for (int q = y + 1; q < map.GetHeight(); q++) {
            if (map.GetBlock(x, q).IsDisabled() || map.GetBlock(x, q).IsUnmovable()) {
                continue;
            }

            return true;
        }

        return false;
    }

    private void MoveEmptyBlockUpArray(int x, int y) {
        for (int q = y + 1; q < map.GetHeight(); q++) {
            if (map.GetBlock(x, q).IsDisabled() || map.GetBlock(x, q).IsUnmovable()) {
                continue;
            }

            Block tempBlock = map.GetBlock(x, y);
            map.SetNewBlock(map.GetBlock(x, q), x, y);
            ;
            map.SetNewBlock(tempBlock, x, q);
            break;
        }
    }

    private void MovePhysicallyBlocksDown(List<Vector2Int> movedBlocks) {
        int movedBLockLength = movedBlocks.Count;

        for (int i = 0; i < movedBLockLength; i++) {
            if (map.GetBlock(movedBlocks[i].x, movedBlocks[i].y).IsDisabled()) {
                continue;
            }

            map.GetBlock(movedBlocks[i].x, movedBlocks[i].y)
               .MoveTo(map.GetWorldPosition(movedBlocks[i].x, movedBlocks[i].y) + map.GetPositionCorrection());
        }
    }

    private void RemoveSweptBlocksFromArray() {
        for (int x = 0; x < map.GetWidth(); x++) {
            for (int y = 0; y < map.GetHeight(); y++) {
                if (!map.GetBlock(x, y).IsDisabled()) {
                    continue;
                }

                map.SetNewBlock(null, x, y);
            }
        }
    }
}