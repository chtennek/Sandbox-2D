using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;
using UnityEditor;

public class PokemonDataSerializer : DataSerializer
{

    protected override void DeserializeData(object data)
    {
        PokemonSaveData save = data as PokemonSaveData;

        // [TODO] optimize this
        GameObject player = GameObject.FindWithTag("Player");
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (player != null)
        {
            player.transform.position = save.position;
            player.SendMessage("ResetDestination");
        }
        if (levelManager != null)
        {
            levelManager.builder.Load(save.level);
        }
    }

    protected override object SerializeData()
    {
        PokemonSaveData save = new PokemonSaveData();

        // [TODO] optimize this
        GameObject player = GameObject.FindWithTag("Player");
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (player != null)
        {
            save.position = player.transform.position;
        }
        if (levelManager != null && levelManager.builder != null)
        {
            save.level = levelManager.builder.GetCurrent();
        }

        return (object)save;
    }
}

[System.Serializable]
public class PokemonSaveData
{
    private SerializableVector3 m_Position;
    private string levelAssetPath;

    public Vector3 position
    {
        get
        {
            return m_Position.ToVector3();
        }
        set {
            m_Position = new SerializableVector3(value);
        }
    }

    public Level level {
        get {
            return AssetDatabase.LoadAssetAtPath<Level>(levelAssetPath);
        }
        set {
            levelAssetPath = AssetDatabase.GetAssetPath(value);
        }
    }

    public PokemonSaveData() { }
    public PokemonSaveData(Vector3 position, Level level)
    {
        this.position = position;
        this.level = level;
    }
}