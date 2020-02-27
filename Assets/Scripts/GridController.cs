using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BlockController))]
public class GridController : MonoBehaviour{
    private Grid grid;
    private BlockController blockController;
    private List<Vector2Int> selectedBlocks = new List<Vector2Int>();
    private BlockColor colorInUse;

    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1;
    [SerializeField] private int minAmountOfSelectedBlocksToDestroy = 3;
    [SerializeField] private float spawnOffset = 1;

    private void Start(){
        blockController = GetComponent<BlockController>();
        blockController.Init(width, height, cellSize);
        Vector3 gridPosition = transform.position - new Vector3(width * 0.5f, height * 0.5f);
        grid = new Grid(width, height, cellSize, gridPosition);
        PopulateBoard();
    }

    // TODO: Hard to navigate. Maybe make box smaller
    public void SweepOverBlock(Vector3 position){
        Vector2Int coordinates = grid.GetXY(position);

        if(!grid.IsWithingGrid(coordinates)) {
            return;
        }

        if(selectedBlocks.Count == 0) {
            colorInUse = blockController.GetBlockColor(coordinates);
            SelectBlock(coordinates);
            return;
        }

        if(selectedBlocks[selectedBlocks.Count - 1] == coordinates) {
            return;
        }

        if(selectedBlocks.Contains(coordinates)) {
            int index = selectedBlocks.IndexOf(coordinates);
            int listLength = selectedBlocks.Count;

            for(int i = index; i < listLength; i++) {
                UnSelectBlock(selectedBlocks[index]);
            }

            return;
        }

        if(grid.IsAdjacentTo(coordinates, selectedBlocks[selectedBlocks.Count - 1]) &&
           blockController.CanBeSelected(coordinates, colorInUse)) {
            SelectBlock(coordinates);
        }
    }

    private void SelectBlock(Vector2Int coordinates){
        blockController.Select(coordinates);
        selectedBlocks.Add(coordinates);
    }

    private void UnSelectBlock(Vector2Int coordinates){
        blockController.Unselect(coordinates);
        selectedBlocks.Remove(coordinates);
    }

    public void SweepEnd(){
        colorInUse = BlockColor.MAX;

        if(selectedBlocks.Count < minAmountOfSelectedBlocksToDestroy) {
            blockController.UnselectAll(selectedBlocks.ToArray());
            selectedBlocks = new List<Vector2Int>();
            return;
        }

        blockController.DeleteBlockAll(selectedBlocks.ToArray());
        blockController.MoveBlocksDown(selectedBlocks.ToArray());
        blockController.RepopulateBoard(selectedBlocks.ToArray());
        selectedBlocks = new List<Vector2Int>();
    }
    
    private void PopulateBoard(){
        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                blockController.SpawnBlock(grid.GetWorldPosition(x, y) + offset, x, y);
            }
        }
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }
}