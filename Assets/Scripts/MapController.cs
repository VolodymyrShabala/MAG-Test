using System.Collections.Generic;

public class MapController {
    private readonly MapData mapData;

    public MapController(MapData mapData) {
        this.mapData = mapData;
    }

    public void HandleSweptBlocks(List<Block> blocks) {
        DisableBlocks(blocks);
        MoveBlocksDown();
        RemoveDisabledBlocksFromMap();
    }

    private void DisableBlocks(IReadOnlyList<Block> blocks) {
        int arrayLength = blocks.Count;

        for (int i = 0; i < arrayLength; i++) {
            blocks[i].SetDisabled();
        }
    }

    private void MoveBlocksDown() {
        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight() - 1; y++) {
                if (mapData.GetBlock(x, y).IsDisabled() && IsThereEnabledBlockAbove(x, y)) {
                    SwapBlockWithFirstEnabledAbove(x, y);
                }
            }
        }
    }

    private bool IsThereEnabledBlockAbove(int x, int y) {
        for (int q = y + 1; q < mapData.GetHeight(); q++) {
            if (!mapData.GetBlock(x, q).IsDisabled() && !mapData.GetBlock(x, q).IsUnmovable()) {
                return true;
            }
        }

        return false;
    }

    private void SwapBlockWithFirstEnabledAbove(int x, int y) {
        for (int q = y + 1; q < mapData.GetHeight(); q++) {
            if (!mapData.GetBlock(x, q).IsDisabled() && !mapData.GetBlock(x, q).IsUnmovable()) {
                Block tempBlock = mapData.GetBlock(x, y);
                mapData.SetNewBlock(mapData.GetBlock(x, q), x, y);
                mapData.SetNewBlock(tempBlock, x, q);
                mapData.GetBlock(x, y).MoveTo(mapData.GetWorldPosition(x, y));
                break;
            }
        }
    }

    private void RemoveDisabledBlocksFromMap() {
        for (int x = 0; x < mapData.GetWidth(); x++) {
            for (int y = 0; y < mapData.GetHeight(); y++) {
                if (mapData.GetBlock(x, y).IsDisabled()) {
                    mapData.SetNewBlock(null, x, y);
                }
            }
        }
    }
}