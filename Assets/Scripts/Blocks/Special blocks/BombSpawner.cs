using UnityEngine;

public class BombSpawner : MonoBehaviour {
    private GameController gameController;
    private BlocksController blocksController;
    [Tooltip("Amount of blocks destroyed to spawn a bomb")]
    [SerializeField] private int amountOfBlocksToSpawnBomb = 5;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
        gameController.onBlocksDestroy += OnBlocksDestroyed;
        // blocksController = FindObjectOfType<BlocksController>();
    }

    private void OnBlocksDestroyed(int value) {
        if (value < amountOfBlocksToSpawnBomb) {
            return;
        }
        
        // blocksController.AddBombRandom();
    }
}