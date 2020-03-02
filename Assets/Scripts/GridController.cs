using UnityEngine;
using System.Collections.Generic;

public class GridController {
    private readonly MapData mapData;

    public GridController(MapData mapData) {
        this.mapData = mapData;
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

        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight() - 1; y++) {
                if (!mapData.GetBlock(x, y).IsDisabled()) {
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
        for (int q = y + 1; q < mapData.GetHeight(); q++) {
            if (mapData.GetBlock(x, q).IsDisabled() || mapData.GetBlock(x, q).IsUnmovable()) {
                continue;
            }

            return true;
        }

        return false;
    }

    private void MoveEmptyBlockUpArray(int x, int y) {
        for (int q = y + 1; q < mapData.GetHeight(); q++) {
            if (mapData.GetBlock(x, q).IsDisabled() || mapData.GetBlock(x, q).IsUnmovable()) {
                continue;
            }

            Block tempBlock = mapData.GetBlock(x, y);
            mapData.SetNewBlock(mapData.GetBlock(x, q), x, y);
            mapData.SetNewBlock(tempBlock, x, q);
            break;
        }
    }

    private void MovePhysicallyBlocksDown(List<Vector2Int> movedBlocks) {
        int movedBLockLength = movedBlocks.Count;

        for (int i = 0; i < movedBLockLength; i++) {
            if (mapData.GetBlock(movedBlocks[i].x, movedBlocks[i].y).IsDisabled()) {
                continue;
            }

            mapData.GetBlock(movedBlocks[i].x, movedBlocks[i].y)
               .MoveTo(mapData.GetWorldPosition(movedBlocks[i].x, movedBlocks[i].y) + mapData.GetPositionCorrection());
        }
    }

    private void RemoveSweptBlocksFromArray() {
        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight(); y++) {
                if (!mapData.GetBlock(x, y).IsDisabled()) {
                    continue;
                }

                mapData.SetNewBlock(null, x, y);
            }
        }
    }
}