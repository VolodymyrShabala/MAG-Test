using UnityEngine;

public class Bomb : MonoBehaviour {
    public BombDirection bombDirection;
    [SerializeField] private Sprite spriteVisualization;

    private void OnEnable() {
        GetComponentInParent<Block>().blockType = BlockType.Bomb;
    }
}