using UnityEngine;
namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SeamlessNoiseGenerator))]
    public class GenerateButtons : MonoBehaviour
    {
#if UNITY_EDITOR

        private SeamlessNoiseGenerator sng;
        // Use this for initialization
        void Start()
        {
            sng = GetComponent<SeamlessNoiseGenerator>();
        }
        public void Create3DTexture()
        {
            sng.Create3DTexture();
        }
        public void Create2DTexture()
        {
            sng.Create2DTexture();
        }
        public void CreateTextureArray()
        {
            sng.CreateTextureArray();
        }
        public void CreateTextureAtlas()
        {
            sng.CreateTextureAtlas();
        }

#endif
    }
}