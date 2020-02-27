using UnityEngine;

public class Grid{
    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private readonly Vector3 originalPosition;

    public Grid(int width, int height, float cellSize, Vector3 originalPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originalPosition = originalPosition;

        bool showDebug = true;

        if(!showDebug) {
            return;
        }

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public bool IsAdjacentTo(Vector2Int block, Vector2Int neighbourBlock){
        for(int i = 0; i < 1; i++) {
            if(Mathf.Abs(block.x - neighbourBlock.x) <= 1 && Mathf.Abs(block.y - neighbourBlock.y) <= 1) {
                return true;
            }
        }

        return false;
    }

    public Vector2Int GetXY(Vector3 worldPosition){
        int x = Mathf.FloorToInt((worldPosition - originalPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originalPosition).y / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x, y) * cellSize + originalPosition;
    }

    public bool IsWithingGrid(Vector2Int coordinates){
        return IsWithingGrid(coordinates.x, coordinates.y);
    }

    public bool IsWithingGrid(int x, int y){
        return x >= 0 && y >= 0 && x < width && y < height;
    }
}