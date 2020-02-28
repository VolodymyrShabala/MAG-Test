using System;
using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private BlockColor blockColor;
    [SerializeField] private float fallSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRenderer{
        get {
            if(spriteRenderer == null) {
                spriteRenderer = GetComponent<SpriteRenderer>();
                originalColor = spriteRenderer.color;
            }

            return spriteRenderer;
        }
    }
    
    private Vector3 position;
    private float moveLerp;
    private Color originalColor;
    private BlockState blockState;
    
    private void Update() {
        if (blockState != BlockState.Moving) {
            return;
        }

        moveLerp += fallSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, position, moveLerp);

        if ((transform.position - position).sqrMagnitude <= 0.05f) {
            transform.position = position;
            moveLerp = 0;
            blockState = BlockState.Idle;
        }
    }

    public void MoveTo(Vector3 position) {
        this.position = position;
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

    public void ReturnToPool(){
        blockState = BlockState.InPool;
    }

    public void SetDisabled(){
        SpriteRenderer.color = originalColor;
        SpriteRenderer.enabled = false;
        blockState = BlockState.Disabled;
    }

    public bool IsInPool(){
        return blockState == BlockState.InPool;
    }
    
    public bool IsDisabled(){
        return blockState != BlockState.Disabled;
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