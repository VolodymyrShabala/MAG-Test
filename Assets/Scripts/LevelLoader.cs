using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour {
    private FileReader fileReader;
    [SerializeField] private TextAsset levelTXT;
    
    private void Start() {
        int[,] level = FileReader.ReadLevel(AssetDatabase.GetAssetPath(levelTXT));

        for (int x = 0; x < level.GetLength(0); x++) {
            string coord = "";
            for (int y = 0; y <level.GetLength(1); y++) {
                coord += $"|{level[x, y].ToString()}";
            }

            Debug.Log($"Level: {coord}");
        }


        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 5; y++) {
                level[x, y] = Random.Range(0, 3);
            }
        }
        FileReader.WriteLevel(AssetDatabase.GetAssetPath(levelTXT), level);
    }

    private void Update() {
        int[,] level = new int[5, 5];

        if (Input.GetKeyDown(KeyCode.K)) {
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    level[x, y] = Random.Range(0, 3);
                }
            }

            FileReader.WriteLevel(AssetDatabase.GetAssetPath(levelTXT), level);
        }
    }
}