using UnityEngine;
using UnityEditor;

namespace WorldSpaceTransitions
{
    [CustomEditor(typeof(GenerateButtons))]
    public class GenerateButtonsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GenerateButtons myScript = (GenerateButtons)target;
           /* if (GUILayout.Button("Generate Texture Array"))
            {
                myScript.CreateTextureArray();
            }
            if (GUILayout.Button("Generate 3D Texture"))
            {
                myScript.Create3DTexture();
            }*/
            if (GUILayout.Button("Generate 2D Texture"))
            {
                myScript.Create2DTexture();
            }
            if (GUILayout.Button("Generate Texture Atlas"))
            {
                myScript.CreateTextureAtlas();
            }
        }
    }
}