using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private BlockColor blockColor;
    [SerializeField] private float fallSpeed = 5;
    private bool hasBeenSwipedOver;
    private Vector3 position;
    private bool doFallDown;

    private void Update() {
        if (!doFallDown) {
            return;
        }
        
        transform.Translate(fallSpeed * Time.deltaTime * Time.deltaTime * Vector3.down);

        if ((transform.position - position).sqrMagnitude <= 0.01f) {
            transform.position = position;
            doFallDown = false;
        }
    }
    
    public void Spawned() {
        
    }

    public void MoveTo(Vector3 position) {
        this.position = position;
        doFallDown = true;
    }
    
    public void SwipedOver() {
        hasBeenSwipedOver = true;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public BlockColor GetBlockColor() {
        return blockColor;
    }
}