using System;
using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private BlockColor blockColor;
    [SerializeField] private float fallSpeed = 0.1f;
    
    private SpriteRenderer spriteRenderer;
    
    private Vector3 position;
    private float moveLerp;
    private Color originalColor;
    private BlockState blockState;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

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

    public void MoveDownOneUnit(){
        position -= new Vector3(0, 1);
        blockState = BlockState.Moving;
    }

    // TODO: Do an animation or something other
    public void Select(){
        blockState = BlockState.Selected;
        spriteRenderer.color = Color.black;
    }

    public void UnSelect(){
        blockState = BlockState.Idle;
        spriteRenderer.color = originalColor;
    }

    public bool CanBeMoved(){
        return blockState == BlockState.Idle;
    }

    public void SetDisabled(){
        blockState = BlockState.Disabled;
    }
    
    public bool Disabled(){
        return blockState == BlockState.Disabled;
    }
    
    public bool CanBeSwipedOver(BlockColor blockColor){
        return this.blockColor == blockColor && blockState == BlockState.Idle;
    }

    public void Reset(){
        UnSelect();
    }

    public BlockColor GetBlockColor(){
        return blockColor;
    }
}