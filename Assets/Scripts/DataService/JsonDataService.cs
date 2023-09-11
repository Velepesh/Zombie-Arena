using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataService : IDataService
{
    public void SaveData<T>(string path, T data)
    {
        string relativePath = Application.persistentDataPath + path;

        try
        {
            if (File.Exists(relativePath))
                File.Delete(relativePath);
            else
                Debug.Log("Writing file for the first time!");
         
            using FileStream stream = File.Create(relativePath);
            stream.Close();
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
        }
    }

    public T LoadData<T>(string path)
    {
        string relativePath = Application.persistentDataPath + path;
        if (File.Exists(relativePath) == false)
        {
            Debug.Log($"Cannot load file at {relativePath}. File does not exist!");
            return default(T);
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(relativePath));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}