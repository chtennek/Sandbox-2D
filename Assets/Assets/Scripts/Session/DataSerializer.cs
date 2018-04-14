using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public abstract class DataSerializer : MonoBehaviour
{
    #region Singleton pattern
    public static DataSerializer current;

    private bool EnsureSingleton()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != null && current != this)
        {
            Destroy(gameObject);
            return false;
        }
        // DontDestroyOnLoad(gameObject);
        return true;
    }
    #endregion

    protected virtual void Awake()
    {
        EnsureSingleton();
    }

    protected abstract void DeserializeData(object data);
    protected abstract object SerializeData();

    public static bool SaveExists()
    {
        return File.Exists(Application.persistentDataPath + "/save.dat");
    }

    public static void Save()
    {
        if (current == null)
        {
            Debug.LogWarning("Save failed! No DataSerializer found.");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        bf.Serialize(file, (object)current.SerializeData());
        file.Close();
    }

    public static void Load()
    {
        if (current == null)
        {
            Debug.LogWarning("Load failed! No DataSerializer found.");
            return;
        }

        if (SaveExists() == false)
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        object data;
        try
        {
            data = bf.Deserialize(file);
        }
        catch (SerializationException)
        {
            Debug.LogWarning("Load failed! Empty save file.");
            file.Close();
            return;
        }
        file.Close();

        current.DeserializeData(data);
    }
}