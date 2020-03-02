using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private BlocksController blocksController;
    private List<Block> selectedBlocks = new List<Block>();
    private BlockType blockInUse;
    [Tooltip("How many blocks are needed to be selected for them to be counted")]
    [SerializeField] private int amountOfSelectedBlocksToDestroy = 3;

    public Action<int> onBlocksDestroy;
    
    private void Start() {
        if (!blocksController) {
            blocksController = FindObjectOfType<BlocksController>();

            if (!blocksController) {
                Debug.LogError($"Block controller is not assigned in {name}.");
            }
        }
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
        blockInUse = BlockType.MAX;

        int listCount = selectedBlocks.Count;
        if (listCount < amountOfSelectedBlocksToDestroy) {
            for (int i = 0; i < listCount; i++) {
                selectedBlocks[i].Deselect();
            }
            selectedBlocks = new List<Block>();
            return;
        }

        blocksController.SweepEnd(selectedBlocks);
        onBlocksDestroy?.Invoke(listCount);
        selectedBlocks = new List<Block>();
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
}