using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private BlocksController blocksController;
    private Grid grid;
    private List<Vector2Int> selectedBlocks = new List<Vector2Int>();
    private BlockColor colorInUse;

    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1;
    [Tooltip("How many blocks are needed to be selected for them to be counted")]
    [SerializeField] private int amountOfSelectedBlocksToDestroy = 3;
    [SerializeField] private float spawnOffset = 1;
    
    public Action<int> onBlocksDestroy;
    
    private void Start() {
        Vector3 position = transform.position - new Vector3(width, height) * 0.5f;
        // blocksController = FindObjectOfType<BlocksController>();
        // blocksController.Init(width, height, cellSize, position, spawnOffset);
        blocksController = new BlocksController(width, height, cellSize, position, spawnOffset);
        grid = new Grid(width, height, cellSize, position);
    }

    public void SweepOverBlock(Vector3 position) {
        Vector2Int coordinates = grid.GetXY(position);

        if (!grid.IsWithingGrid(coordinates)) {
            return;
        }

        if (selectedBlocks.Count == 0) {
            colorInUse = blocksController.GetBlockColor(coordinates);
            SelectBlock(coordinates);
            return;
        }

        if (selectedBlocks[selectedBlocks.Count - 1] == coordinates) {
            return;
        }

        if (selectedBlocks.Contains(coordinates)) {
            int index = selectedBlocks.IndexOf(coordinates);
            int listLength = selectedBlocks.Count;

            for (int i = index; i < listLength; i++) {
                UnSelectBlock(selectedBlocks[index]);
            }
            return;
        }

        if (grid.IsAdjacentTo(coordinates, selectedBlocks[selectedBlocks.Count - 1]) &&
            blocksController.CanBeSelected(coordinates, colorInUse)) {
            SelectBlock(coordinates);
        }
    }

    private void SelectBlock(Vector2Int coordinates) {
        blocksController.Select(coordinates);
        selectedBlocks.Add(coordinates);
    }

    private void UnSelectBlock(Vector2Int coordinates) {
        blocksController.Unselect(coordinates);
        selectedBlocks.Remove(coordinates);
    }

    public void SweepEnd() {
        colorInUse = BlockColor.MAX;

        if (selectedBlocks.Count < amountOfSelectedBlocksToDestroy) {
            blocksController.UnselectAll(selectedBlocks.ToArray());
            selectedBlocks = new List<Vector2Int>();
            return;
        }

        blocksController.SweepEnd(selectedBlocks.ToArray());
        // onBlocksDestroy?.Invoke(selectedBlocks.Count);
        selectedBlocks = new List<Vector2Int>();
    }
}