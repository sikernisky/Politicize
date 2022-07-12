using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
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

    private void Update()
    {
        Save();
    }



    public void Save()
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            data = new PlayerData();
            Save();
            return;
        }

        string loadedData = File.ReadAllText(path);
        data = JsonConvert.DeserializeObject<PlayerData>(loadedData);
    }

}
