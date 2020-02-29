using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField] private BlockColor blockColor = BlockColor.Blue;
    [SerializeField] private float fallSpeed = 0.1f;
    public BlockType blockType = BlockType.ColorBlock;

    private SpriteRenderer spriteRenderer;

    private SpriteRenderer SpriteRenderer {
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
        if (blockType == BlockType.InvisibleObstacle || blockType == BlockType.DestructibleObstacle) {
            print($"Trying to move unmovable block");
            return;
        }

        this.positionToMoveTo = position;
        blockState = BlockState.Moving;
    }

    // TODO: Do an animation or something other
    public void Select() {
        if (blockType != BlockType.ColorBlock && blockType != BlockType.Bomb) {
            print("Trying select unselectable block");
            return;
        }

        blockState = BlockState.Selected;
        SpriteRenderer.color = Color.black;
    }

    public void UnSelect() {
        if (blockType != BlockType.ColorBlock && blockType != BlockType.Bomb) {
            print("Trying to unselect unselectable block");
            return;
        }

        blockState = BlockState.Idle;
        SpriteRenderer.color = originalColor;
    }

    public void SetDisabled() {
        if (blockType != BlockType.ColorBlock && blockType != BlockType.Bomb) {
            print("Trying to disable unselectable block.");
            return;
        }

        SpriteRenderer.color = originalColor;
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

    public bool CanBeSelected(BlockColor blockColor) {
        return this.blockColor == blockColor && blockState == BlockState.Idle &&
               (blockType == BlockType.ColorBlock || blockType == BlockType.Bomb);
    }

    public BlockColor GetBlockColor() {
        return blockColor;
    }

    public bool IsUnmovable() {
        return blockType == BlockType.DestructibleObstacle || blockType == BlockType.InvisibleObstacle;
    }
}