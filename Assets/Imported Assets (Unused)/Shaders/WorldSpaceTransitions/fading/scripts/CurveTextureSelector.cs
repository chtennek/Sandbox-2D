using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldSpaceTransitions
{
    public class CurveTextureSelector : MonoBehaviour
    {

        public List<Texture2D> curveTextures;
        public GameObject curveTextureTemplate;

        void Start()
        {
            setupCurveTextures();
        }


        void setupCurveTextures()
        {
            if (FadingTransition.instance.transitionTexture != null)
            {
                curveTextures.Insert(0, FadingTransition.instance.transitionTexture);
            }

            for (int i = 0; i < curveTextures.Count; i++)
            {
                GameObject newItem = Instantiate(curveTextureTemplate);
                newItem.transform.SetParent(curveTextureTemplate.transform.parent);
                newItem.transform.SetSiblingIndex(1);
                newItem.SetActive(true);
                newItem.GetComponentInChildren<RawImage>().texture = curveTextures[i];
                Toggle t = newItem.GetComponent<Toggle>();
                Texture2D tex = curveTextures[i];
                t.onValueChanged.AddListener(delegate
                {
                    Shader.SetGlobalTexture("_Curves", tex);
                    //Debug.Log(tex.name);
                });
                t.isOn = (i == curveTextures.Count - 1);

            }
        }
    }
}
