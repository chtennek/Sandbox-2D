using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpritePostprocessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
        if (asset)
            return; //set default values only for new textures;

        TextureImporter importer = assetImporter as TextureImporter;
        //set your default settings to the importer here

        importer.filterMode = FilterMode.Point;
    }

}