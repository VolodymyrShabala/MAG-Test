using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridBuilder))]
public class GridBuilderEditor : Editor {
    private GridBuilder grid;
    private void OnEnable() {
        grid = target as GridBuilder;
    }

    // Create buttons to load and save levels
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Save level")) {
            grid.SaveLevel();
        }
        
        if (GUILayout.Button("Load level")) {
            grid.LoadLevel();
        }
    }
    
    private void OnSceneGUI() {
        // Draw buttons to choose block type
        Handles.BeginGUI();
        int numberOfBlocks = (int) BlockType.MAX;
        int positionY = 10;

        for (int i = 0; i < numberOfBlocks; i++) {
            if (GUI.Button(new Rect(10, positionY, 125, 50), $"{(BlockType) i}")) {
                grid.blockType = (BlockType) i;
            }

            positionY += 60;
        }

        GUI.Label(new Rect(500, 0, 300, 50), "Hold left shift and press left mouse button on a grid");

        Handles.EndGUI();

        // Draw a grid
        Handles.color = Color.red;
        int width = grid.height;
        int height = grid.height;
        float cellSize = grid.cellSize;
        Vector3 position = grid.transform.position;

        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                Handles.DrawLine(GetWorldPosition(x, y, cellSize, position),
                                 GetWorldPosition(x, y + 1, cellSize, position));

                Handles.DrawLine(GetWorldPosition(x, y, cellSize, position),
                                 GetWorldPosition(x + 1, y, cellSize, position));
            }

            Handles.DrawLine(GetWorldPosition(0, height, cellSize, position),
                             GetWorldPosition(width, height, cellSize, position));

            Handles.DrawLine(GetWorldPosition(width, 0, cellSize, position),
                             GetWorldPosition(width, height, cellSize, position));
        }

        // TODO: Return wrong X|Y coordinates
        // Change block type and prevent deselecting this gameObject
        if (Event.current.shift && Event.current.type == EventType.MouseDown) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Event.current.mousePosition);
            Vector2Int gridValue = GetXY(mousePosition, cellSize, position);
            // Debug.Log($"{gridValue.x}|{gridValue.y}");

            if (IsWithingGrid(gridValue.x, gridValue.y, width, height)) {
                grid.levelGrid[gridValue.x, gridValue.y] = (int) grid.blockType;
            }
        }

        Selection.activeGameObject = grid.gameObject;
        
        // Draw all blocks
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Handles.color = GetBLockColor(grid.levelGrid[x, y]);
                Handles.DrawWireCube(GetWorldPosition(x, y, cellSize, position) + new Vector3(cellSize * 0.5f, cellSize * 0.5f), Vector3.one);
            }
        }
    }

    private Color GetBLockColor(int value) {
        switch (value) {
            case 0:
                return Color.green;
            case 1:
                return Color.blue;
            case 2:
                return Color.red;
            case 3:
                return  Color.cyan;
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

    private Vector2Int GetXY(Vector3 worldPosition, float cellSize, Vector3 position) {
        int x = Mathf.FloorToInt((worldPosition - position).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - position).y / cellSize);

        return new Vector2Int(x, y);
    }
}