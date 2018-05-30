using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ScriptableObjectDictionary : SerializableDictionary<string, ScriptableObject> { }

[Serializable]
public class TilemapDataDictionary : SerializableDictionary<string, Levels.LevelTilemapData> { }

[Serializable]
public class StringToMenuDictionary : SerializableDictionary<string, GameMenu> { }
