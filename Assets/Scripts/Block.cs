using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private BlockColor blockColor;
    [SerializeField] private float fallSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer{
        get {
            if (spriteRenderer != null)
                return spriteRenderer;

            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;

            return spriteRenderer;
        }
    }
    
    private Color originalColor;
    private Vector3 positionToMoveTo;
    private float moveLerpValueHolder;
    private BlockState blockState;
    
    private void Update() {
        if (blockState != BlockState.Moving) {
            return;
        }

        moveLerpValueHolder += fallSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, positionToMoveTo, moveLerpValueHolder);

        if ((transform.position - positionToMoveTo).sqrMagnitude <= 0.05f) {
            transform.position = positionToMoveTo;
            moveLerpValueHolder = 0;
            blockState = BlockState.Idle;
        }
    }

    public void MoveTo(Vector3 position) {
        this.positionToMoveTo = position;
        blockState = BlockState.Moving;
    }

    // TODO: Do an animation or something other
    public void Select(){
        blockState = BlockState.Selected;
        SpriteRenderer.color = Color.black;
    }

    public void UnSelect(){
        blockState = BlockState.Idle;
        SpriteRenderer.color = originalColor;
    }

    public void SetDisabled(){
        SpriteRenderer.color = originalColor;
        SpriteRenderer.enabled = false;
        blockState = BlockState.Disabled;
    }

    public bool IsDisabled(){
        return blockState == BlockState.Disabled;
    }

    public void SetEnabled(){
        blockState = BlockState.Idle;
        SpriteRenderer.enabled = true;
    }
    
    public bool CanBeSelected(BlockColor blockColor){
        return this.blockColor == blockColor && blockState == BlockState.Idle;
    }

    public BlockColor GetBlockColor(){
        return blockColor;
    }
}