using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour {
    [SerializeField] private Transform[] blockPrefabs;
    private Grid grid;
    private Block[,] blocksArray;
    private List<Vector2Int> swipedBlocks = new List<Vector2Int>();
    private Action onSweepEnd;

    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1;

    [Tooltip("How high initial spawn point is for block to fall down from.")]
    [SerializeField] private float spawnHeight = 5;

    // TODO: Rename
    private Vector3 spawnHeightVector;

    private void Start() {
        spawnHeightVector = new Vector3(0, spawnHeight);
        Vector3 gridPosition = transform.position - new Vector3(width * 0.5f, height * 0.5f);
        grid = new Grid(width, height, cellSize, gridPosition);
        blocksArray = new Block[width, height];
        PopulateBoard();
    }

    public void SweepOverBlock(Vector3 position) {
        Vector2Int coordinates = grid.GetXY(position);

        if (!swipedBlocks.Contains(coordinates) && grid.IsWithingGrid(coordinates)) {
            swipedBlocks.Add(coordinates);
        }
    }

    public void SweepEnd() {
        foreach (Vector2Int block in swipedBlocks) {
            blocksArray[block.x, block.y].DestroySelf();
        }

        SpawnNewBlocks();
    }

    private void SpawnNewBlocks() {
        foreach (Vector2Int block in swipedBlocks) {
            int randomBlock = Random.Range(0, blockPrefabs.Length);

            Vector3 truePosition = grid.GetWorldPosition(block.x, block.y) +
                                   new Vector3(cellSize * 0.5f, cellSize * 0.5f);

            blocksArray[block.x, block.y] = Instantiate(blockPrefabs[randomBlock],
                                                        truePosition + spawnHeightVector, Quaternion.identity,
                                                        transform).GetComponent<Block>();

            blocksArray[block.x, block.y].MoveTo(truePosition);
        }

        swipedBlocks = new List<Vector2Int>();
    }

    private void PopulateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int randomBlock = Random.Range(0, blockPrefabs.Length);
                Vector3 truePosition = grid.GetWorldPosition(x, y) + new Vector3(cellSize * 0.5f, cellSize * 0.5f);

                blocksArray[x, y] = Instantiate(blockPrefabs[randomBlock], truePosition + spawnHeightVector,
                                                Quaternion.identity, transform).GetComponent<Block>();
                blocksArray[x, y].MoveTo(truePosition);

            }
        }
    }
}