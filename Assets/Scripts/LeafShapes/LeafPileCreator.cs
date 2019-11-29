#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor window and script to create a leaf pile prefab
/// </summary>
public class LeafPileCreator : EditorWindow
{
    private Texture2D _texture;
    private Vector3Int _dimensions;
    private bool _transparency;

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
