using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataService : IDataService
{
    string _relativePath = Application.persistentDataPath + "/save.json";

    public void SaveData(SaveData Data)
    {
        try
        {
            if (File.Exists(_relativePath))
            {
                Debug.Log("Data exist. Deleting old file and writing a new one!");
                File.Delete(_relativePath);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }
         
            using FileStream stream = File.Create(_relativePath);
            stream.Close();
            File.WriteAllText(_relativePath, JsonConvert.SerializeObject(Data, Formatting.Indented));
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
        }
    }

    public SaveData LoadData()
    {
        if (File.Exists(_relativePath) == false)
        {
            Debug.Log($"Cannot load file at {_relativePath}. File does not exist!");
            return new SaveData();
        }

        try
        {
            SaveData data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(_relativePath));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}