using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileReader {
    // [MenuItem("Tools/Write file")]
    // static void WriteString() {
    //     string path = "Assets/Resources/test.txt";
    //
    //     //Write some text to the test.txt file
    //     StreamWriter writer = new StreamWriter(path, true);
    //     writer.WriteLine("Test");
    //     writer.Close();
    //
    //     //Re-import the file to update the reference in the editor
    //     AssetDatabase.ImportAsset(path);
    //     TextAsset asset = (TextAsset) Resources.Load("test");
    //
    //     //Print the text from the file
    //     Debug.Log(asset.text);
    // }
    //
    // [MenuItem("Tools/Read file")]
    // static void ReadString() {
    //     string path = "Assets/Resources/test.txt";
    //
    //     //Read the text from directly from the test.txt file
    //     StreamReader reader = new StreamReader(path);
    //     Debug.Log(reader.ReadToEnd());
    //     reader.Close();
    // }

    public static int[,] ReadLevel(TextAsset file) {
        return ReadLevel(AssetDatabase.GetAssetPath(file));
    }

    public static int[,] ReadLevel(string filePath) {
        string input;
        try {
            input = File.ReadAllText(filePath);
        } catch {
            Debug.LogError($"Couldn't read the file {filePath}.");
            return default;
        }

        string[] lines = input.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

        int height = lines.Length;
        int width = lines[0].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Length;

        int[,] level = new int[width, height];

        for (int i = 0; i < height; i++) {
            string st = lines[i];
            string[] numbers = st.Split(',');

            for (int j = 0; j < width; j++) {
                if (int.TryParse(numbers[j], out int value)) {
                    level[j,  (height - 1) - i] = value;
                } else {
                    level[j, i] = -1;
                }
            }
        }

        return level;
    }

    public static void WriteLevel(TextAsset file, int[,] level) {
        WriteLevel(AssetDatabase.GetAssetPath(file), level);
    }

    public static void WriteLevel(string filePath, int[,] level) {
        string content = "";

        int xLength = level.GetLength(0);
        int yLength = level.GetLength(1);

        for (int x = xLength - 1; x >= 0; x--) {
            for (int y = 0; y < yLength; y++) {
                string coma = ",";

                if (y == yLength - 1) {
                    coma = "";
                }

                content += $"{level[y, x]}" + coma;
            }

            if (x == 0) {
                break;
            }
            content += "|\r\n";
        }

        File.WriteAllText(filePath, content);
    }
}