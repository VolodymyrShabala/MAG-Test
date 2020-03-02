using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksController : MonoBehaviour {
    [SerializeField] private TextAsset loadLevelTXT;
    [SerializeField] private Block blockPrefab;

    [Header("Default values if no save assigned")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1;
    [Tooltip("How many blocks are needed to be selected for them to be counted")]
    [SerializeField] private float spawnOffset = 1;

    private Block[,] blocksArray;
    private Vector3 positionCorrection;
    private Vector3 originalPosition;

    private List<Block> savedBlockForReuse = new List<Block>();
    private BoardState boardState;

    private void Start() {
        int[,] level = new int[width, height];

        if (loadLevelTXT) {
            level = FileReader.ReadLevel(loadLevelTXT);
            width = level.GetLength(0);
            height = level.GetLength(1);
        }

        originalPosition = transform.position - new Vector3(width, height) * 0.5f;
        positionCorrection = new Vector3(cellSize * 0.5f, cellSize * 0.5f);
        blocksArray = new Block[width, height];

        PopulateBoard(loadLevelTXT ? level : null);
    }

    public void SweepEnd(List<Block> blocks) {
        if (boardState == BoardState.SpawningNewBlocks) {
            return;
        }

        boardState = BoardState.DisablingBlocks;
        SaveSweptOverBlocksForReuse(blocks);
        StopCoroutine(HandleSweepEnd());
        StartCoroutine(HandleSweepEnd());
    }

    private IEnumerator HandleSweepEnd() {
        DisableSweptOverBlocks();
        print("Coroutine started");
        yield return new WaitForSeconds(0.1f);

        boardState = BoardState.SpawningNewBlocks;
        MoveBlocksDown();
        RemoveSweptBlocksFromArray();
        RepopulateBoard();
        ResetReusableBlocksList();
        boardState = BoardState.Waiting;
    }

    private void SaveSweptOverBlocksForReuse(List<Block> blocks) {
        savedBlockForReuse.AddRange(blocks);
    }

    private void DisableSweptOverBlocks() {
        int arrayLength = savedBlockForReuse.Count;

        for (int i = 0; i < arrayLength; i++) {
            if (savedBlockForReuse[i].IsDisabled()) {
                continue;
            }

            savedBlockForReuse[i].SetDisabled();
        }
    }

    private void MoveBlocksDown() {
        List<Vector2Int> blocksToMoveDown = MoveBlocksDownArrayAndReturnTheirCoordinates();
        MovePhysicallyBlocksDown(blocksToMoveDown);
    }

    private List<Vector2Int> MoveBlocksDownArrayAndReturnTheirCoordinates() {
        List<Vector2Int> blocksToMoveDown = new List<Vector2Int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height - 1; y++) {
                if (!blocksArray[x, y].IsDisabled()) {
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
        for (int q = y + 1; q < height; q++) {
            if (blocksArray[x, q].IsDisabled() || blocksArray[x, q].IsUnmovable()) {
                continue;
            }

            return true;
        }

        return false;
    }

    private void MoveEmptyBlockUpArray(int x, int y) {
        for (int q = y + 1; q < height; q++) {
            if (blocksArray[x, q].IsDisabled() || blocksArray[x, q].IsUnmovable()) {
                continue;
            }

            Block tempBlock = blocksArray[x, y];
            blocksArray[x, y] = blocksArray[x, q];
            blocksArray[x, q] = tempBlock;
            break;
        }
    }

    private void MovePhysicallyBlocksDown(List<Vector2Int> movedBlocks) {
        int movedBLockLength = movedBlocks.Count;

        for (int i = 0; i < movedBLockLength; i++) {
            if (blocksArray[movedBlocks[i].x, movedBlocks[i].y].IsDisabled()) {
                continue;
            }

            blocksArray[movedBlocks[i].x, movedBlocks[i].y]
                    .MoveTo(GetWorldPosition(movedBlocks[i].x, movedBlocks[i].y) + positionCorrection);
        }
    }

    private void RemoveSweptBlocksFromArray() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (!blocksArray[x, y].IsDisabled()) {
                    continue;
                }

                blocksArray[x, y] = null;
            }
        }
    }

    private void RepopulateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (blocksArray[x, y]) {
                    continue;
                }

                SpawnBlock(Random.Range(0, (int) BlockType.DestructibleObstacle), x, y);
            }
        }
    }

    private void PopulateBoard(int[,] loadedLevel = null) {
        bool level = loadedLevel != null;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                SpawnBlock(level ? loadedLevel[x, y] : Random.Range(0, (int) BlockType.MAX), x, y);
            }
        }
    }

    private void SpawnBlock(int blockType, int x, int y) {
        Vector3 gridPosition = GetWorldPosition(x, y) + positionCorrection;
        Vector3 spawnPosition = GetWorldPosition(x, height) + positionCorrection + Vector3.up * spawnOffset;

        Block block;
        bool reusedBlock = false;

        if (savedBlockForReuse.Count > 0) {
            block = savedBlockForReuse[0];
            reusedBlock = true;
        } else {
            block = Instantiate(blockPrefab, transform);
        }

        block.SetEnabled();
        block.transform.position = spawnPosition;
        block.MoveTo(gridPosition);
        block.SetBlockType((BlockType) blockType);
        blocksArray[x, y] = block;

        if (reusedBlock) {
            savedBlockForReuse.RemoveAt(0);
        }
    }

    private void ResetReusableBlocksList() {
        savedBlockForReuse = new List<Block>();
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originalPosition;
    }
}

public enum BoardState { Waiting, DisablingBlocks, SpawningNewBlocks }