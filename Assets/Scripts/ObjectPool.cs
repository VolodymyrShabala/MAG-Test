using System.Collections.Generic;
using UnityEngine;

// TODO: Maybe I don't need this. Can just say to block to do this.
public class ObjectPool : MonoBehaviour {
    [SerializeField] private Block[] blockPrefabs;
    private Dictionary<BlockColor, Block> blocks = new Dictionary<BlockColor, Block>();

    public Block SpawnBlock(BlockColor blockColor) {
        foreach (KeyValuePair<BlockColor,Block> keyValuePair in blocks) {
            if (keyValuePair.Key == blockColor && !blocks[keyValuePair.Key].gameObject.activeInHierarchy) {
                return blocks[keyValuePair.Key];
            }
        }

        Block block = Instantiate(blockPrefabs[0]);
        blocks.Add(block.GetBlockColor(), block);

        return block;
    }
}