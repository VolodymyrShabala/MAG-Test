using System.Collections.Generic;
using UnityEngine;

public class BlocksController : MonoBehaviour {
    [SerializeField] private TextAsset loadLevelTXT;
    [SerializeField] private Block blockPrefab;

    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1;
    [SerializeField] private float spawnOffset = 1;
    
    private Map map;
    private GridController gridController;
    private BlockCreator blockCreator;
    
    private void Start() {
        Transform myTransform = transform;
        map = new Map(loadLevelTXT, width, height, cellSize, myTransform.position);

        gridController = new GridController(map);
        blockCreator = new BlockCreator(map, blockPrefab, myTransform, spawnOffset);
    }

    public void SweepEnd(List<Block> blocks) {
        gridController.HandleSweptBlocks(blocks);
        blockCreator.RepopulateBoard(blocks);
    }
}