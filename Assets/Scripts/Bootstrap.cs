using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private GameController gameController;

    private void Start() {
        if (!gameConfig) {
            Debug.LogError($"No game config was assigned in {name}. Aborting game start.");
            return;
        }

        if (!gameController) {
            gameController = FindObjectOfType<GameController>();

            if (!gameController) {
                Debug.LogError($"There is no Game Controller in the scene. Aborting game start.");
                return;
            }
        }

        int[,] levelData = MapCreator.CreateOrGenerateLevel(gameConfig);

        Transform myTransform = transform;

        MapData mapData = new MapData(gameConfig.GetBlockPrefab(), gameConfig.GetCellSize(),
                                      gameConfig.GetSpawnOffset(), myTransform.position, levelData);

        BlockCreator blockCreator = new BlockCreator(mapData, myTransform);
        GridController gridController = new GridController(mapData);
        gameController.Init(blockCreator, gridController, gameConfig.GetAmountOfSelectedBlocksToDestroy());
    }
}