using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private TouchController touchController;

    private void Start() {
        if (!gameConfig) {
            Debug.LogError($"No game config was assigned in {name}. Aborting game start.");
            return;
        }

        if (!touchController) {
            touchController = FindObjectOfType<TouchController>();

            if (!touchController) {
                Debug.LogError($"No game Touch Controller was assigned in {name}. Aborting game start.");
                return;
            }
        }

        int[,] levelData = MapCreator.CreateOrGenerateLevel(gameConfig);
        Transform myTransform = transform;

        MapData mapData = new MapData(gameConfig.GetCellSize(), myTransform.position, levelData);
        BlockCreator blockCreator = new BlockCreator(mapData, gameConfig.GetSpawnOffset(), gameConfig.GetBlockPrefab(), myTransform);
        MapController mapController = new MapController(mapData);
        GameController gameController = new GameController(blockCreator, mapController, gameConfig.GetAmountOfSelectedBlocksToDestroy());
        touchController.Init(gameController);
    }
}