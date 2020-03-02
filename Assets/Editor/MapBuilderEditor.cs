using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapBuilder))]
public class MapBuilderEditor : Editor {
    private MapBuilder mapBuilder;
    private int width;
    private int height;
    private float cellSize;
    private Vector3 gridPosition;

    private void OnEnable() {
        mapBuilder = target as MapBuilder;
        if (!mapBuilder) {
            Debug.LogError($"There is something wrong in {name}");
            return;
        }
        
        width = mapBuilder.height;
        height = mapBuilder.height;
        cellSize = mapBuilder.cellSize;
        gridPosition = mapBuilder.transform.position;
    }

    // Create buttons to load and save levels
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Save level")) {
            mapBuilder.SaveLevel();
        }

        if (GUILayout.Button("Load level")) {
            mapBuilder.LoadLevel();
        }
    }

    private void OnSceneGUI() {
        Handles.BeginGUI();
        DrawBlockTypeButtons();
        Handles.EndGUI();

        DrawGrid();

        // Receive input and prevent deselecting this gameObject
        if (Event.current.shift && Event.current.type == EventType.MouseDown) {
            ChangeBlockType();
        }

        Selection.activeGameObject = mapBuilder.gameObject;

        DrawAllBlocks();
    }

    private void DrawBlockTypeButtons() {
        int numberOfBlocks = (int) BlockType.MAX;
        int positionY = 10;

        for (int i = 0; i < numberOfBlocks; i++) {
            if (GUI.Button(new Rect(10, positionY, 125, 50), $"{(BlockType) i}")) {
                mapBuilder.blockType = (BlockType) i;
            }

            positionY += 60;
        }

        GUI.Label(new Rect(500, 0, 300, 50), "Hold left shift and press left mouse button on a grid");
    }

    private void DrawGrid() {
        Handles.color = Color.red;

        for (int x = 0; x < mapBuilder.width; x++) {
            for (int y = 0; y < mapBuilder.height; y++) {
                Handles.DrawLine(GetWorldPosition(x, y, cellSize, gridPosition),
                                 GetWorldPosition(x, y + 1, cellSize, gridPosition));

                Handles.DrawLine(GetWorldPosition(x, y, cellSize, gridPosition),
                                 GetWorldPosition(x + 1, y, cellSize, gridPosition));
            }

            Handles.DrawLine(GetWorldPosition(0, height, cellSize, gridPosition),
                             GetWorldPosition(width, height, cellSize, gridPosition));

            Handles.DrawLine(GetWorldPosition(width, 0, cellSize, gridPosition),
                             GetWorldPosition(width, height, cellSize, gridPosition));
        }
    }

    private void ChangeBlockType() {
        Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        Vector2Int gridValue = GetXY(mousePosition, cellSize, gridPosition);

        if (IsWithingGrid(gridValue.x, gridValue.y, width, height)) {
            mapBuilder.levelGrid[gridValue.x, gridValue.y] = (int) mapBuilder.blockType;
        }
    }

    private void DrawAllBlocks() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mapBuilder.levelGrid == null) {
                    return;
                }
                
                Handles.color = GetBlockColor(mapBuilder.levelGrid[x, y]);

                Handles.DrawWireCube(GetWorldPosition(x, y, cellSize, gridPosition) + new Vector3(cellSize * 0.5f, cellSize * 0.5f),
                                     Vector3.one);
            }
        }
    }

    private Color GetBlockColor(int value) {
        switch (value) {
            case 0:
                return Color.green;
            case 1:
                return Color.blue;
            case 2:
                return Color.red;
            case 3:
                return Color.cyan;
            case 4:
                return Color.white;
            default:
                return Color.black;
        }
    }

    private Vector3 GetWorldPosition(int x, int y, float cellSize, Vector3 position) {
        return new Vector3(x, y) * cellSize + position;
    }

    private bool IsWithingGrid(int x, int y, int width, int height) {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private Vector2Int GetXY(Vector3 worldPosition, float cellSize, Vector3 initialPosition) {
        int x = Mathf.FloorToInt((worldPosition - initialPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - initialPosition).y / cellSize);

        return new Vector2Int(x, y);
    }
}