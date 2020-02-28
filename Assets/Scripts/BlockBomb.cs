using UnityEngine;

public class BlockBomb : MonoBehaviour {
    [SerializeField] private BombDirection bombDirection;
    [SerializeField] private Sprite spriteVisualization;

    private void OnEnable() {
        print("Bomb is inbound!");
    }
}