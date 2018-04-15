using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class SeamlessNoiseGenerator : MonoBehaviour {

        public enum NoiseType { quad, pixel };
        public NoiseType noiseType = NoiseType.quad;

        [Range(2, 512)]
        public int resolution3d = 256;

        [Range(2, 512)]
        public int arrayResolution = 256;

        [Range(2, 256)]
        public int arrayDepth = 256;

        [Range(0.1f, 1)]
        public float boxSize = 0.8f;

        private int boxPxSize = 80;

        public int deepness = 10;

        public int delta = 10;

        public float persistence = 0.5f;

        [Range(2, 2048)]
        public int resolution2d = 1024;


        public string textureName = "seamless_";

        private float mins = 1f;
        private float maxs = 0f;
        private int Iterations = 0;


#if UNITY_EDITOR
    public void Create3DTexture()
    {
        boxPxSize = Mathf.RoundToInt(boxSize*resolution3d);
        float[,,] Values3D = new float[resolution3d, resolution3d, resolution3d];

        for (int i = 0; i < resolution3d; i++)
        {
            for (int j = 0; j < resolution3d; j++)
            {
                for (int k = 0; k < resolution3d; k++)
                {
                    Values3D[i,j,k]= 0;
                }
            }
        }

        if (noiseType == NoiseType.quad)
        {
            Debug.Log("m " + mins.ToString());
            Iterations = 0;
            int x = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
            int y = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
            int z = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
            printQuad3D(Values3D, resolution3d, resolution3d, resolution3d, x, y, z, boxPxSize, deepness, delta);
        }

        Texture3D t = new Texture3D(resolution3d, resolution3d, resolution3d, TextureFormat.RGB24, false);
        Color[] cols = new Color[resolution3d * resolution3d * resolution3d];

        Texture2D preview = new Texture2D(resolution3d, resolution3d, TextureFormat.RGB24, false);
        Color[] previewCols = new Color[resolution3d * resolution3d];

        int idx = 0;
        int prevIdx = 0;
        for (int i = 0; i < resolution3d; i++)
        {
            for (int j = 0; j < resolution3d; j++)
            {
                for (int k = 0; k < resolution3d; k++, idx ++)
                {
                    float cc = Values3D[i, j, k]/256f;
                    if (noiseType == NoiseType.pixel) cc = Random.value;
                    cols[idx] = new Color(cc,cc,cc,1);                    
                    mins = Mathf.Min(mins, cc);
                    maxs = Mathf.Max(maxs, cc);
                    if (j == Mathf.RoundToInt(resolution3d / 2))
                    {
                        previewCols[prevIdx] = new Color(cc, cc, cc, 1);
                        prevIdx++;
                    }
                }
            }
        }
        t.SetPixels(cols);
        t.Apply();
        Debug.Log("min "+mins.ToString());
        Debug.Log("max " + maxs.ToString());
        Debug.Log("it " + Iterations.ToString());

        string fname = textureName + "_" + resolution2d.ToString("0");
        if (noiseType == NoiseType.quad) fname += "_" + boxSize.ToString() + "_" + deepness.ToString("0") + "_" + delta.ToString("0");

        string path = "Assets/WorldSpaceTransitions/fading/textures/";
        AssetDatabase.CreateAsset(t, path + fname + "_3D.asset");
        AssetDatabase.Refresh();

        preview.SetPixels(previewCols);
        preview.Apply();
        byte[] bytes = preview.EncodeToPNG();
        File.WriteAllBytes(path + fname + "_2Dpreview.png", bytes);

        AssetDatabase.Refresh();
    }


    public void CreateTextureAtlas()
    {
        if (resolution3d > 256)
        {
                Debug.LogWarning("max resolution is 256");
                return;
        }
        boxPxSize = Mathf.RoundToInt(boxSize * resolution3d);
        float[,,] Values3D = new float[resolution3d, resolution3d, resolution3d];

        for (int i = 0; i < resolution3d; i++)
        {
            for (int j = 0; j < resolution3d; j++)
            {
                for (int k = 0; k < resolution3d; k++)
                {
                    Values3D[i, j, k] = 0;
                }
            }
        }
        if (noiseType == NoiseType.quad)
        {
            Debug.Log("m " + mins.ToString());
            Iterations = 0;
                int x = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                int y = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                int z = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
                printQuad3D(Values3D, resolution3d, resolution3d, resolution3d, x, y, z, boxPxSize, deepness, delta);
        }
        int atlasSize = (int)Mathf.Sqrt(resolution3d);
        int atlasRes = resolution3d * atlasSize;
            Debug.Log("atlasRes " + atlasRes.ToString());
            Texture2D t = new Texture2D(atlasRes, atlasRes, TextureFormat.RGB24, false);

        
        for (int k = 0; k < resolution3d; k++)
        {
            int idx = 0;
            Color[] cols = new Color[resolution3d * resolution3d];
            for (int j = 0; j < resolution3d; j++)
            {
                for (int i = 0; i < resolution3d; i++)
                {
                    float cc = Values3D[i, j, k] / 256f;
                    if (noiseType == NoiseType.pixel) cc = Random.value;
                    cols[idx] = new Color(cc, cc, cc, 1);
                    mins = Mathf.Min(mins, cc);
                    maxs = Mathf.Max(maxs, cc);
                    idx++;
                }
            }
            int x1 = resolution3d * (k % atlasSize);
            int y1 = atlasSize * (k - (k % atlasSize));
            Debug.Log("x1 " + x1.ToString() + " y1 " + y1.ToString());
            t.SetPixels(x1, y1, resolution3d, resolution3d, cols, 0);
        }

        t.Apply();
        Debug.Log("min " + mins.ToString());
        Debug.Log("max " + maxs.ToString());
        Debug.Log("it " + Iterations.ToString());

        string fname = textureName + "_" + resolution2d.ToString("0");
        if (noiseType == NoiseType.quad) fname += "_" + boxSize.ToString() + "_" + deepness.ToString("0") + "_" + delta.ToString("0");
        string path = "Assets/WorldSpaceTransitions/fading/textures/";
        byte[] bytes = t.EncodeToPNG();
        File.WriteAllBytes(path + fname + "_atlas.png", bytes);

        AssetDatabase.Refresh();

        TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(path + fname + "_atlas.png");
        A.textureCompression = TextureImporterCompression.Uncompressed;

        if (noiseType == NoiseType.pixel) A.filterMode = FilterMode.Point;
        A.maxTextureSize = 8192;
        A.mipmapEnabled = false;
        AssetDatabase.ImportAsset(path + fname + "2D.png", ImportAssetOptions.ForceUpdate);
    
    }

    public void CreateTextureArray()
    {
            boxPxSize = Mathf.RoundToInt(boxSize * arrayResolution);
            float[,,] Values3D = new float[arrayResolution, arrayResolution, arrayResolution];

            for (int i = 0; i < arrayResolution; i++)
            {
                for (int j = 0; j < arrayResolution; j++)
                {
                    for (int k = 0; k < arrayResolution; k++)
                    {
                        Values3D[i, j, k] = 0;
                    }
                }
            }

            if (noiseType == NoiseType.quad)
            {
                Debug.Log("m " + mins.ToString());
                Iterations = 0;
                int x = Mathf.RoundToInt(Random.value * (arrayResolution - 0.5f));
                int y = Mathf.RoundToInt(Random.value * (arrayResolution - 0.5f));
                int z = Mathf.RoundToInt(Random.value * (arrayResolution - 0.5f));
                printQuad3D(Values3D, arrayResolution, arrayResolution, arrayResolution, x, y, z, boxPxSize, deepness, delta);
            }
            Texture2DArray t = new Texture2DArray(arrayResolution, arrayResolution, arrayDepth, TextureFormat.RGB24, false);
            //Color[] cols = new Color[resolution3d * resolution3d * resolution3d];

            //Texture2D preview = new Texture2D(resolution3d, resolution3d, TextureFormat.RGB24, false);
            //Color[] previewCols = new Color[resolution3d * resolution3d];

            int idx = 0;
            //int prevIdx = 0;
            for (int k = 0; k < arrayResolution; k++)
            {
                if (k * arrayResolution / arrayDepth<idx) continue;
                Color[] cols = new Color[arrayResolution * arrayResolution];
                int i1 = 0;
                for (int j = 0; j < arrayResolution; j++)
                {
                    for (int i = 0; i < arrayResolution; i++)
                    {
                        float cc = Values3D[i, j, k] / 256f;
                        if (noiseType == NoiseType.pixel) cc = Random.value;
                        cols[i1] = new Color(cc, cc, cc, 1);
                        mins = Mathf.Min(mins, cc);
                        maxs = Mathf.Max(maxs, cc);
                        i1++;
                        //if (j == Mathf.RoundToInt(arrayResolution / 2))
                        //{
                        //    previewCols[prevIdx] = new Color(cc, cc, cc, 1);
                        //    prevIdx++;
                        //}
                    }
                }
                t.SetPixels(cols,idx);
                idx++;
            }
            t.Apply();
            Debug.Log("min " + mins.ToString());
            Debug.Log("max " + maxs.ToString());
            Debug.Log("it " + Iterations.ToString());

            string fname = textureName + "_" + resolution2d.ToString("0");
            if (noiseType == NoiseType.quad) fname += "_" + boxSize.ToString() + "_" + deepness.ToString("0") + "_" + delta.ToString("0");

            string path = "Assets/WorldSpaceTransitions/fading/textures/";
            AssetDatabase.CreateAsset(t, path + fname + "_2Darray.asset");
            AssetDatabase.Refresh();

            //preview.SetPixels(previewCols);
            //preview.Apply();
            //byte[] bytes = preview.EncodeToPNG();
            //File.WriteAllBytes(path + fname + "_2Dpreview.png", bytes);

            //AssetDatabase.Refresh();
    }




        private void printQuad3D(float[,,] Values, int wid, int hei, int dep, int x, int y, int z, int boxPxSize, int deepness, int delta)
    {

        if (boxPxSize > wid)
            boxPxSize = wid;
        if (boxPxSize > hei)
            boxPxSize = wid;
        //if (boxPxSize > dep)
        //    dep = wid;


        if (deepness > 0 && boxPxSize >= 1)
        {
            for (int i = -boxPxSize / 2; i < boxPxSize / 2; i++)
            {
                for (int j = -boxPxSize / 2; j < boxPxSize / 2; j++)
                {
                    for (int k = -boxPxSize / 2; k < boxPxSize / 2; k++)
                    {

                        /*seamless management start*/
                        int pixX = (x + i) % wid;
                        int pixY = (y + j) % hei;
                        int pixZ = (z + k) % dep;
                        if (pixX < 0)
                            pixX = wid + pixX;
                        if (pixY < 0)
                            pixY = hei + pixY;
                        if (pixZ < 0)
                            pixZ = dep + pixZ;
                        /*seamless management end*/

                        Values[pixX, pixY, pixZ] = (Values[pixX, pixY, pixZ] + delta)%256; // add value

                        //Iterations++;
                    }
                }
                Iterations++;
                int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxPxSize * 0.5), x + (int)(boxPxSize * 0.5)));
                int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxPxSize * 0.5), y + (int)(boxPxSize * 0.5)));
                int zz = Mathf.RoundToInt(Random.Range(z - (int)(boxPxSize * 0.5), z + (int)(boxPxSize * 0.5)));

                printQuad3D(Values, wid, hei, dep, xx, yy, zz, boxPxSize / 2, --deepness, delta);
            }
        }
    }

    public void Create2DTexture()
    {
        boxPxSize = Mathf.RoundToInt(boxSize * resolution2d);
        float[,] Values2D = new float[resolution2d, resolution2d];

        for (int i = 0; i < resolution3d; i++)
        {
            for (int j = 0; j < resolution3d; j++)
            {
                Values2D[i, j] = 0;
            }
        }

        if (noiseType == NoiseType.quad)
        {
             Debug.Log("m " + mins.ToString());
             Iterations = 0;
             int x = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
             int y = Mathf.RoundToInt(Random.value * (resolution3d - 0.5f));
             printQuad(Values2D, resolution2d, resolution2d, x, y, boxPxSize, deepness, delta);
        }
        Texture2D t = new Texture2D(resolution2d, resolution2d, TextureFormat.RGB24, false);
        Color[] cols = new Color[resolution2d * resolution2d];

        int idx = 0;
        for (int i = 0; i < resolution2d; i++)
        {
            for (int j = 0; j < resolution2d; j++, idx++)
            {
                 float cc = Values2D[i, j] / 256f;
                 if (noiseType == NoiseType.pixel) cc = Random.value;
                 cols[idx] = new Color(cc, cc, cc, 1);
                 mins = Mathf.Min(mins, cc);
                 //Debug.Log(mins.ToString()+" ||| "+cc.ToString());
                 maxs = Mathf.Max(maxs, cc);
            }
        }
        t.SetPixels(cols);
        t.Apply();
        Debug.Log("min " + mins.ToString());
        Debug.Log("max " + maxs.ToString());
        Debug.Log("it " + Iterations.ToString());

        string fname = textureName + "_" + resolution2d.ToString("0"); 
        if (noiseType == NoiseType.quad) fname += "_" + boxSize.ToString() + "_" + deepness.ToString("0") + "_" + delta.ToString("0");
        byte[] bytes = t.EncodeToPNG();
        string path = Application.dataPath + "/WorldSpaceTransitions/fading/textures/";
        File.WriteAllBytes(path + fname + "2D.png", bytes);

        AssetDatabase.Refresh();
        TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(path + fname + "2D.png");
        A.textureCompression = TextureImporterCompression.Uncompressed;
        A.filterMode = FilterMode.Point;
        A.mipmapEnabled = false;
        AssetDatabase.ImportAsset(path + fname + "2D.png", ImportAssetOptions.ForceUpdate);
        }


    private void printQuad(float[,] Values, int wid, int hei, int x, int y, int boxPxSize, int deepness, int delta)
    {


        if (boxPxSize > wid)
            boxPxSize = wid;
        if (boxPxSize > hei)
            boxPxSize = wid;

        if (deepness > 0 && boxPxSize >= 1)
        {
            for (int i = -boxPxSize / 2; i < boxPxSize / 2; i++)
            {
                for (int j = -boxPxSize / 2; j < boxPxSize / 2; j++)
                {
                    /*seamless management start*/
                    int pixX = (x + i) % wid;
                    int pixY = (y + j) % hei;
                    if (pixX < 0)
                        pixX = wid + pixX;
                    if (pixY < 0)
                        pixY = wid + pixY;
                    /*seamless management end*/

                    Values[pixX, pixY] = (Values[pixX, pixY] + delta)%256; // add value
/*                  if(Values[pixX, pixY] + delta < 256)
                    {
                        Values[pixX, pixY] = Values[pixX, pixY] + delta;
                    }
                    else
                    {
                        Values[pixX, pixY] = Values[pixX, pixY] - delta;
                    }
*/
                }

                int xx = Mathf.RoundToInt(Random.Range(x - (int)(boxPxSize * 0.5), x + (int)(boxPxSize * 0.5)));
                int yy = Mathf.RoundToInt(Random.Range(y - (int)(boxPxSize * 0.5), y + (int)(boxPxSize * 0.5)));

                Iterations++;
                printQuad(Values, wid, hei, xx, yy, boxPxSize / 2, --deepness, delta);
            }
        }
    }


#endif
    }
}
