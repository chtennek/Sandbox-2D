using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

public class AudioAutoplay : EditorWindow
{

    [MenuItem("Tools/Audio Autoplay")]
    static void Init()
    {
        var window = EditorWindow.GetWindow(typeof(AudioAutoplay));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Audio files will now play on selection change.");
    }

    void OnSelectionChange()
    {
        UnityEngine.Object[] clips = Selection.GetFiltered(typeof(AudioClip), SelectionMode.Unfiltered);

        if (clips != null && clips.Length == 1)
        {
            AudioClip clip = (AudioClip)clips[0];
            PlayClip(clip);
        }

    }

    /*
     * How to play audio in Editor using an Editor script
     * 
     * http://forum.unity3d.com/threads/way-to-play-audio-in-editor-using-an-editor-script.132042/
     */
    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] {
             typeof(AudioClip)
        },
        null
        );
        method.Invoke(
            null,
            new object[] {
             clip
        }
        );
    } // PlayClip()
}