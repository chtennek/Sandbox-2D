using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    public virtual void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SaveData data = new SaveData();

        bf.Serialize(file, data);
        file.Close();
    }

    public virtual void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat") == false)
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();
    }
}

[System.Serializable]
public class SaveData
{
    Vector3 playerPosition;
    Levels.Level currentLevel;
}