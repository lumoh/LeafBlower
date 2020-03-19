using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Audio entry for the Audio Manager
/// </summary>
[Serializable]
public class AudioEntry
{
    public string Name;
    public string Description;
    public AudioClip AudioClip;

    [Range(0f, 1f)] public float Volume = 1f;

    /// <summary>
    /// Get a audio clip or if variations exist, pick a random one
    /// </summary>
    /// <returns></returns>
    public virtual AudioClip GetAudioClip()
    {
        AudioClip clip = AudioClip;
        return clip;
    }
}

/// <summary>
/// Music has an intro which plays into the loop.
/// </summary>
[Serializable]
public class MusicAudioEntry : AudioEntry
{
    public AudioClip Intro;
}

/// <summary>
/// Music has an intro which plays into the loop.
/// </summary>
[Serializable]
public class SFXAudioEntry : AudioEntry
{
    public List<AudioClip> Variations;

    /// <summary>
    /// Get a audio clip or if variations exist, pick a random one
    /// </summary>
    /// <returns></returns>
    public override AudioClip GetAudioClip()
    {
        AudioClip clip = AudioClip;

        if (Variations.Count > 0)
        {
            int randIndex = UnityEngine.Random.Range(-1, Variations.Count);
            if (randIndex != -1)
            {
                clip = Variations[randIndex];
            }
        }

        return clip;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AudioClip))]
public class AudioEntryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float propertyHeight = 20;
        float buttonWidth = 40;
        var audioClipRect = new Rect(position.x, position.y, position.width - (buttonWidth * 2), propertyHeight);
        var testRect = new Rect(position.x + audioClipRect.width, position.y, buttonWidth, propertyHeight);
        var stopRect = new Rect(position.x + audioClipRect.width + buttonWidth, position.y, buttonWidth, propertyHeight);

        EditorGUI.PropertyField(audioClipRect, property, GUIContent.none);
        if (GUI.Button(testRect, "Play"))
        {
            AudioClip audioClip = property.objectReferenceValue as AudioClip;
            if (audioClip != null)
            {
                StopAllClips();
                PlayClip(audioClip);
            }
        }

        if (GUI.Button(stopRect, "Stop"))
        {
            StopAllClips();
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null,
            new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }

    public static void StopAllClips()
    {
        System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null,
            new System.Type[] { },
            null
        );
        method.Invoke(
            null,
            new object[] { }
        );
    }
}
#endif