using UnityEngine;

public static class MapCreator {
    public static int[,] CreateOrGenerateLevel(GameConfig gameConfig) {
        if (!gameConfig.GetLevelFile()) {
            return GenerateRandomLevel(gameConfig.GetWidth(), gameConfig.GetHeight());
        }
        
        return FileReader.ReadLevel(gameConfig.GetLevelFile());
    }

    private static int[,] GenerateRandomLevel(int width, int height) {
        int[,] data = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                data[x, y] = Random.Range(0, (int) BlockType.DestructibleObstacle);
            }
        }

        return data;
    }
}