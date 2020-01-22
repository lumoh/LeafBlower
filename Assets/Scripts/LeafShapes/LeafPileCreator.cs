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

    private string[] platformPaths;

    private Vector3 prevPosition;
    private bool doSnap = true;
    private float snapValue = 1;

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
        GUILayout.Label("Snapping Placement", EditorStyles.boldLabel);
        doSnap = EditorGUILayout.Toggle("Auto Snap", doSnap);
        snapValue = EditorGUILayout.FloatField("Snap Value", snapValue);

        GUILayout.Space(10);
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        drawButtonsForLeafPiles();

        GUILayout.Space(10);
        GUILayout.Label("Platforms", EditorStyles.boldLabel);

        drawButtonsForPlatforms();
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

    private void drawButtonsForLeafPiles()
    {
        dirPaths = Directory.GetDirectories(Application.dataPath + "/Prefabs/Leaves");
        dirNames = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirNames[i] = dirPaths[i].Substring(dirPaths[i].LastIndexOf('/') + 1);
        }


        for (int i = 0; i < dirNames.Length; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label(dirNames[i] + ":");
            filePaths = Directory.GetFiles(dirPaths[i]);

            bool horizontalLayout;
            for (int k = 0; k < filePaths.Length; k++)
            {
                horizontalLayout = k % 4 == 0;
                string fileName = filePaths[k].Substring(filePaths[k].LastIndexOf('/') + 1);
                if (fileName.EndsWith("prefab", System.StringComparison.InvariantCulture))
                {
                    string strippedFileName = fileName.Replace(".prefab", "");
                    if(horizontalLayout)
                    {
                        GUILayout.BeginHorizontal();
                    }
                    if (GUILayout.Button(strippedFileName))
                    {
                        PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                        if (stage != null)
                        {
                            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(filePaths[k].Substring(filePaths[k].IndexOf("Assets", System.StringComparison.InvariantCulture)));
                            GameObject spawnPrefab = PrefabUtility.InstantiatePrefab(asset, stage.scene) as GameObject;
                            GameObject[] roots = stage.scene.GetRootGameObjects();
                            if (roots.Length > 0)
                            {
                                Level level = roots[0].GetComponent<Level>();
                                if (level != null)
                                {
                                    spawnPrefab.transform.parent = level.LeavesParent;
                                    EditorSceneManager.MarkSceneDirty(stage.scene);
                                }
                            }
                        }
                    }
                    if (!horizontalLayout)
                    {
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }

    private void drawButtonsForPlatforms()
    {
        GUILayout.BeginVertical();
        platformPaths = Directory.GetFiles(Application.dataPath + "/Prefabs/LevelEditor");
        for (int k = 0; k < platformPaths.Length; k++)
        {
            string fileName = platformPaths[k].Substring(platformPaths[k].LastIndexOf('/') + 1);
            if (fileName.EndsWith("prefab", System.StringComparison.InvariantCulture))
            {
                string strippedFileName = fileName.Replace(".prefab", "");
                if (GUILayout.Button(strippedFileName))
                {
                    PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (stage != null)
                    {
                        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(platformPaths[k].Substring(platformPaths[k].IndexOf("Assets", System.StringComparison.InvariantCulture)));
                        GameObject spawnPrefab = PrefabUtility.InstantiatePrefab(asset, stage.scene) as GameObject;
                        GameObject[] roots = stage.scene.GetRootGameObjects();
                        if (roots.Length > 0)
                        {
                            Level level = roots[0].GetComponent<Level>();
                            if (level != null)
                            {
                                spawnPrefab.transform.parent = level.PlatformsParent;
                                EditorSceneManager.MarkSceneDirty(stage.scene);
                            }
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (doSnap
             && !EditorApplication.isPlaying
             && Selection.transforms.Length > 0
             && Selection.transforms[0].position != prevPosition &&
             Selection.transforms[0].name.Contains("Platform"))
        {
            Snap();
            prevPosition = Selection.transforms[0].position;
        }
    }

    private void Snap()
    {
        foreach (var transform in Selection.transforms)
        {
            var t = transform.transform.position;
            t.x = Round(t.x);
            t.y = Round(t.y);
            t.z = Round(t.z);
            transform.transform.position = t;
        }
    }

    private float Round(float input)
    {
        return snapValue * Mathf.Round((input / snapValue));
    }
}
#endif
