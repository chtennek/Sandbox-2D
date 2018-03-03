using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomPropertyDrawer(typeof(ScriptableObjectDictionary))]
[CustomPropertyDrawer(typeof(TilemapDataDictionary))]
[CustomPropertyDrawer(typeof(StringToMenuDictionary))]
public class DictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }