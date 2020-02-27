using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour{
    [SerializeField] private Block[] blockPrefabs;
    private readonly Dictionary<BlockColor, List<Block>> blocksDictionary = new Dictionary<BlockColor, List<Block>>();
    
    private static ObjectPool instance;
    public static ObjectPool GetInstance => instance;

    private void Awake(){
        if(instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        int colorsLength = (int)BlockColor.MAX;
        for(int i = 0; i < colorsLength; i++) {
            blocksDictionary[(BlockColor) i] = new List<Block>();
        }
        
        GridController grid = FindObjectOfType<GridController>();
        int spawnAmount = grid ? grid.GetWidth() * grid.GetHeight() : 25;

        ExpandPool(spawnAmount);
    }

    public Block GetBlock(BlockColor blockColor){
        foreach(BlockColor color in blocksDictionary.Keys) {
            if(color != blockColor) {
                continue;
            }

            int listLength = blocksDictionary[color].Count;
            for(int i = 0; i < listLength; i++) {
                Block block = blocksDictionary[color][i];

                if(!block.gameObject.activeInHierarchy) {
                    block.gameObject.SetActive(true);
                    return block;
                }
            }
        }

        ExpandPool();
        return GetBlock(blockColor);
    }

    private void ExpandPool(int spawnAmount = 5){
        int prefabsLength = blockPrefabs.Length;

        for(int i = 0; i < prefabsLength; i++) {
            for(int q = 0; q < spawnAmount; q++) {
                Block block = Instantiate(blockPrefabs[i], transform);
                block.gameObject.SetActive(false);
                blocksDictionary[block.GetBlockColor()].Add(block);
            }
        }
    }

    public void ReturnToPool(Block block){
        block.Reset();
        block.gameObject.SetActive(false);
    }
}