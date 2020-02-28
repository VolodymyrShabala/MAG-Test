using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockController))]
public class GameController : MonoBehaviour{
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
        blockController.Init(width, height, cellSize, transform.position, spawnOffset);
    }

    // TODO: Hard to navigate. Maybe make box smaller
    public void SweepOverBlock(Vector3 position){
        Vector2Int coordinates = blockController.GetXY(position);

        if(!blockController.IsWithingGrid(coordinates)) {
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

        if(blockController.IsAdjacentTo(coordinates, selectedBlocks[selectedBlocks.Count - 1]) &&
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

        blockController.DeleteBlocksAll(selectedBlocks.ToArray());
        blockController.MoveBlocksDown(selectedBlocks.ToArray());
        blockController.RepopulateBoard(selectedBlocks.ToArray());
        blockController.ReturnBlocksToPool();
        selectedBlocks = new List<Vector2Int>();
         }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }
}