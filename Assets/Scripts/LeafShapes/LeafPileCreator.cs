#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

/// <summary>
/// Editor window and script to create a leaf pile prefab
/// </summary>
public class LeafPileCreator : EditorWindow
{
    private Texture2D _texture;
    private Vector3Int _dimensions;
    private bool _transparency;

    private string[] dirNames;
    private string[] dirPaths;
    private string[] filePaths;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Blower/Leaf Pile Creator")]
    static void Init()
    {
        LeafPileCreator window = (LeafPileCreator)EditorWindow.GetWindow(typeof(LeafPileCreator));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Leaf Pile Creator", EditorStyles.boldLabel);

        _texture = (Texture2D)EditorGUILayout.ObjectField("Image", _texture, typeof(Texture2D), allowSceneObjects: true);
        _transparency = EditorGUILayout.Toggle("Transparency", _transparency);
        if (GUILayout.Button("Create From Image"))
        {
            createFromImage();
        }

        _dimensions = EditorGUILayout.Vector3IntField("Dimensions", _dimensions);
        if (GUILayout.Button("Create From Dimensions"))
        {
            createFromDimensions();
        }

        GUILayout.Space(10);
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        dirPaths = Directory.GetDirectories(Application.dataPath + "/Prefabs/Leaves");
        dirNames = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirNames[i] = dirPaths[i].Substring(dirPaths[i].LastIndexOf('/') + 1);
        }

        for(int i = 0; i < dirNames.Length; i++)
        {
            GUILayout.Label(dirNames[i] + ":");
            filePaths = Directory.GetFiles(dirPaths[i]);

            for (int k = 0; k < filePaths.Length; k++)
            {
                string fileName = filePaths[k].Substring(filePaths[k].LastIndexOf('/') + 1);
                if (fileName.EndsWith("prefab", System.StringComparison.InvariantCulture))
                {
                    string strippedFileName = fileName.Replace(".prefab", "");
                    if (GUILayout.Button(strippedFileName))
                    {
                        PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                        if (stage != null)
                        {
                            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(filePaths[k].Substring(filePaths[k].IndexOf("Assets", System.StringComparison.InvariantCulture)));
                            GameObject spawnPrefab = PrefabUtility.InstantiatePrefab(asset, stage.scene) as GameObject;
                            GameObject[] roots = stage.scene.GetRootGameObjects();
                            if(roots.Length > 0)
                            {
                                Level level = roots[0].GetComponent<Level>();
                                spawnPrefab.transform.parent = level.LeavesParent;
                                EditorSceneManager.MarkSceneDirty(stage.scene);                                
                            }
                        }
                    }
                }
            }
        }
    }

    private void createFromImage()
    {
        if(_texture != null)
        {
            Leaf leafPrefab = AssetDatabase.LoadAssetAtPath<Leaf>("Assets/Prefabs/Leaves/Leaf.prefab");
            GameObject leafPileObj = new GameObject();
            LeafBitmap leafBitmap = leafPileObj.AddComponent<LeafBitmap>();
            leafBitmap.LeafPrefab = leafPrefab;
            leafBitmap.Bitmap = _texture;
            leafBitmap.CreateLeavesFromBitmap(_transparency);
            PrefabUtility.SaveAsPrefabAsset(leafPileObj, "Assets/Prefabs/Leaves/LeafPile_" + _texture.name + ".prefab");
            DestroyImmediate(leafPileObj);
        }
    }

    private void createFromDimensions()
    {
        if (_dimensions.x > 0 && _dimensions.y > 0 && _dimensions.z > 0)
        {
            Leaf leafPrefab = AssetDatabase.LoadAssetAtPath<Leaf>("Assets/Prefabs/Leaves/Leaf.prefab");
            GameObject leafPileObj = new GameObject();
            LeafCube leafBitmap = leafPileObj.AddComponent<LeafCube>();
            leafBitmap.LeafPrefab = leafPrefab;
            leafBitmap.Dimensions = _dimensions;
            leafBitmap.CreateFromDimensions();
            PrefabUtility.SaveAsPrefabAsset(leafPileObj, "Assets/Prefabs/Leaves/LeafPile_" + _dimensions.x + "x" + _dimensions.y + "x" + _dimensions.z + ".prefab");
            DestroyImmediate(leafPileObj);
        }
    }
}
#endif
