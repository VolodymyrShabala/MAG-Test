using System.Collections.Generic;
using UnityEngine;

public class GameController {
    private readonly BlockCreator blockCreator;
    private readonly MapController mapController;

    private List<Block> selectedBlocks = new List<Block>();
    private BlockType blockInUse;
    private readonly int amountOfSelectedBlocksToDestroy;
    private bool isSweeping;

    public GameController(BlockCreator blockCreator, MapController mapController, int amountOfSelectedBlocksToDestroy) {
        this.amountOfSelectedBlocksToDestroy = amountOfSelectedBlocksToDestroy;
        this.blockCreator = blockCreator;
        this.mapController = mapController;
    }

    public void SweepOverBlock(Block block) {
        if (IsListEmpty()) {
            AddFirstElement(block);
            return;
        }

        if (IsTheLastElement(block)) {
            return;
        }

        if (IsInTheList(block)) {
            if (IsNextToLastElement(block)) {
                RemoveLastSelected();
            }
        }

        if (!IsAdjacentToLastBlock(block)) {
            return;
        }

        if (block.CanBeSelectedWithColor(blockInUse)) {
            SelectBlock(block);
        }
    }

    private void SelectBlock(Block block) {
        block.Select();
        selectedBlocks.Add(block);
    }

    private void DeselectBlock(Block block) {
        block.Deselect();
        selectedBlocks.Remove(block);
    }

    public void SweepEnd() {
        int listCount = selectedBlocks.Count;

        if (listCount < amountOfSelectedBlocksToDestroy) {
            for (int i = 0; i < listCount; i++) {
                selectedBlocks[i].Deselect();
            }
        } else {
            mapController.HandleSweptBlocks(selectedBlocks);
            blockCreator.RepopulateBoard(selectedBlocks);
        }

        Reset();
    }

    private bool IsListEmpty() {
        return selectedBlocks.Count == 0;
    }

    private void AddFirstElement(Block block) {
        if (!block.CanBeSelected()) {
            return;
        }

        blockInUse = block.GetBlockType();
        SelectBlock(block);
    }

    private bool IsTheLastElement(Block block) {
        return selectedBlocks[selectedBlocks.Count - 1] == block;
    }

    private bool IsInTheList(Block block) {
        return selectedBlocks.Contains(block);
    }

    private bool IsNextToLastElement(Block block) {
        int blockIndex = selectedBlocks.IndexOf(block);
        return blockIndex == selectedBlocks.Count - 2;
    }

    private void RemoveLastSelected() {
        DeselectBlock(selectedBlocks[selectedBlocks.Count - 1]);
    }

    private bool IsAdjacentToLastBlock(Block block) {
        Vector3 blockPosition = block.transform.position;
        Vector3 neighbourBlockPosition = selectedBlocks[selectedBlocks.Count - 1].transform.position;
        return Mathf.Abs(Vector3.Distance(blockPosition, neighbourBlockPosition)) <= 1.5f;
    }

    private void Reset() {
        blockInUse = BlockType.MAX;
        selectedBlocks = new List<Block>();
    }
}