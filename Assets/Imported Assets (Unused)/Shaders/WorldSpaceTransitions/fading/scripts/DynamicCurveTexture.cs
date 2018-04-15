using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class DynamicCurveTexture : MonoBehaviour
    {

        public static DynamicCurveTexture instance;

        //public AnimationCurve alphaChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        //public AnimationCurve redChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        //public AnimationCurve greenChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        //public AnimationCurve blueChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        public Gradient gradient = new Gradient();

        public static Texture2D texture;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            UpdateCurvesTexture();
            if (!FadingTransition.instance) return;
            if (FadingTransition.instance.useDynamicTexture)
            {
                Shader.SetGlobalTexture("_Curves", texture);
            }

        }

        void UpdateCurvesTexture()
        {
            if (texture == null)
            {
                texture = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
                texture.wrapMode = TextureWrapMode.Clamp;
            }
            for (int i = 0; i < 256; i++)
            {
                float x = i * 1.0f / 255;
                Color col = new Color();
                float aCh;

                //float rCh = Mathf.Clamp(redChannel.Evaluate(x), 0.0f, 1.0f);
                //float gCh = Mathf.Clamp(greenChannel.Evaluate(x), 0.0f, 1.0f);
                //float bCh = Mathf.Clamp(blueChannel.Evaluate(x), 0.0f, 1.0f);
                //aCh = Mathf.Clamp(alphaChannel.Evaluate(x), 0.0f, 1.0f);
                //col = new Color(rCh, bCh, gCh);

                if (gradient != null) col = gradient.Evaluate(x);
                aCh = col.a;

                texture.SetPixel(i, 0, col);
                texture.SetPixel(i, 1, col);
                texture.SetPixel(i, 2, col);
                texture.SetPixel(i, 3, new Color(aCh, aCh, aCh));
            }

            texture.Apply();
#if UNITY_EDITOR

            byte[] bytes = texture.EncodeToPNG();
            string path = Application.dataPath + "/WorldSpaceTransitions/fading/textures/";
            File.WriteAllBytes(path + "_curves.png", bytes);
            AssetDatabase.Refresh();

            path = "Assets/WorldSpaceTransitions/fading/textures/_curves.png";

            TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(path);
            A.textureCompression = TextureImporterCompression.Uncompressed;
            A.filterMode = FilterMode.Point;
            A.wrapMode = TextureWrapMode.Clamp;
            A.mipmapEnabled = false;
            //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            //TransitionTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
#endif
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            if (FadingTransition.instance == null) return;
            if (FadingTransition.instance.useDynamicTexture)
                UpdateCurvesTexture();
            Shader.SetGlobalTexture("_Curves", texture);
        }
#endif
    }
}