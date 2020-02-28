using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    [SerializeField] private Block[] blockPrefabs;
    private readonly Dictionary<BlockColor, List<Block>> blocksDictionary = new Dictionary<BlockColor, List<Block>>();

    private static ObjectPool instance;
    public static ObjectPool GetInstance => instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        int colorsLength = (int) BlockColor.MAX;

        for (int i = 0; i < colorsLength; i++) {
            blocksDictionary[(BlockColor) i] = new List<Block>();
        }
        
        ExpandPool(25);
    }

    public Block GetBlock(BlockColor blockColor) {
        int listLength = blocksDictionary[blockColor].Count;
        List<Block> blockList = blocksDictionary[blockColor];

        for (int i = 0; i < listLength; i++) {
            Block block = blockList[i];

            if (!block.gameObject.activeInHierarchy) {
                block.gameObject.SetActive(true);
                return block;
            }
        }

        if (blocksDictionary[blockColor].Capacity == 0) {
            Debug.LogError($"Requested a block of color that is not assigned in block prefabs in {name}");
            return default;
        }

        ExpandPool();

        return GetBlock(blockColor);
    }

    private void ExpandPool(int spawnAmount = 5) {
        int prefabsLength = blockPrefabs.Length;

        for (int i = 0; i < prefabsLength; i++) {
            for (int q = 0; q < spawnAmount; q++) {
                Block block = Instantiate(blockPrefabs[i], transform);
                block.gameObject.SetActive(false);
                block.SetDisabled();
                blocksDictionary[block.GetBlockColor()].Add(block);
            }
        }
    }

    public void ReturnToPool(Block block) {
        block.gameObject.SetActive(false);
    }
}