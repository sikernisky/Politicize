using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



/// <summary>
/// Saves and Loads data.
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary>The current "load" of saved data.</summary>
    public static PlayerData data;

    /// <summary>The path of the save file.</summary>
    private string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/playerdata.json";
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            data = new PlayerData();
            Save();
        }

        string loadedData = File.ReadAllText(path);
        data = JsonUtility.FromJson<PlayerData>(loadedData);
    }
}
