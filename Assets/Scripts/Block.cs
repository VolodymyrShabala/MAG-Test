using System;
using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private float fallSpeed = 0.1f;
    [SerializeField] private BlockType blockType = BlockType.Blue;
    private BlockState blockState;

    private SpriteRenderer spriteRenderer;

    private SpriteRenderer SpriteRenderer {
        get {
            if (spriteRenderer != null)
                return spriteRenderer;

            spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
    }

    private Color originalColor;
    private Vector3 positionToMoveTo;
    private float moveLerpValueHolder;

    private void Update() {
        if (blockState != BlockState.Moving) {
            return;
        }

        moveLerpValueHolder += fallSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, positionToMoveTo, moveLerpValueHolder);

        if ((transform.position - positionToMoveTo).sqrMagnitude <= 0.01f) {
            transform.position = positionToMoveTo;
            moveLerpValueHolder = 0;
            blockState = BlockState.Idle;
        }
    }

    public void MoveTo(Vector3 position) {
        if (IsUnmovable()) {
            return;
        }

        positionToMoveTo = position;
        blockState = BlockState.Moving;
    }

    public void Select() {
        if (IsUnmovable()) {
            return;
        }

        blockState = BlockState.Selected;
        SpriteRenderer.color = Color.black;
    }

    public void Deselect() {
        if (IsUnmovable()) {
            return;
        }

        blockState = BlockState.Idle;
        SpriteRenderer.color = originalColor;
    }

    public void SetDisabled() {
        if (IsUnmovable()) {
            return;
        }

        SpriteRenderer.enabled = false;
        blockState = BlockState.Disabled;
    }

    public bool IsDisabled() {
        return blockState == BlockState.Disabled;
    }

    public void SetEnabled() {
        blockState = BlockState.Idle;
        SpriteRenderer.enabled = true;
    }

    // TODO: Look into this. Bad to have two function doing the same stuff
    public bool CanBeSelected() {
        return blockState == BlockState.Idle && !IsUnmovable();
    }
    
    public bool CanBeSelectedWithColor(BlockType blockType) {
        return this.blockType == blockType && blockState == BlockState.Idle && !IsUnmovable();
    }

    public BlockType GetBlockType() {
        return blockType;
    }

    public bool IsUnmovable() {
        return blockType == BlockType.DestructibleObstacle || blockType == BlockType.Invisible;
    }

    public void SetBlockType(BlockType type) {
        SpriteRenderer.enabled = true;

        switch (type) {
            case BlockType.Green:
                SpriteRenderer.color = Color.green;
                blockType = BlockType.Green;
                break;
            case BlockType.Blue:
                SpriteRenderer.color = Color.blue;
                blockType = BlockType.Blue;
                break;
            case BlockType.Red:
                SpriteRenderer.color = Color.red;
                blockType = BlockType.Red;
                break;
            case BlockType.DestructibleObstacle:
                blockType = BlockType.DestructibleObstacle;
                SpriteRenderer.color = Color.cyan;
                GetComponent<Collider2D>().enabled = false;
                break;
            case BlockType.Invisible:
                blockType = BlockType.Invisible;
                SpriteRenderer.enabled = false;
                GetComponent<Collider2D>().enabled = false;
                break;
            default:
                Debug.LogError("Block received wrong state. Setting it to Green");
                SpriteRenderer.color = Color.green;
                blockType = BlockType.Green;
                break;
        }

        originalColor = SpriteRenderer.color;
    }
}

public enum BlockState { Idle, Selected, Moving, Disabled }
public enum BlockType { Green, Blue, Red, DestructibleObstacle, Invisible, MAX }