using System;
using UnityEngine;

public class GridAdjustment : MonoBehaviour {
    [SerializeField] private SpawnSpecialBlock[] specialGridAdjustments;

    private void Start() {
        int arrayLength = specialGridAdjustments.Length;

        if (arrayLength == 0) {
            return;
        }

        BlocksController blockController = FindObjectOfType<BlocksController>();

        for (int i = 0; i < arrayLength; i++) {
            blockController.SetBlockToType(specialGridAdjustments[i].blockState, specialGridAdjustments[i].position);
        }
    }
}

[Serializable]
public struct SpawnSpecialBlock {
    public BlockType blockState;
    public Vector2Int position;
}