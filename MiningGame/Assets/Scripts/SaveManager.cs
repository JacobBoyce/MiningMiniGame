using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static void Save<T>(T objectToSave, string key)
    {
        string json = JsonUtility.ToJson(objectToSave);

        string path = Application.persistentDataPath + key + ".sav";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using(StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(json);
        }
        Debug.Log("Save successful");
    }

    public static T Load<T>(string key)
    {
        string json;
        string path = Application.persistentDataPath + key + ".sav";
        if(File.Exists(path))
        {
            using(StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }
            Debug.Log("Load successful");
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogWarning("File Not Found!");
        }

        return default(T);
    }

    public static void DeleteFile(string file)
    {
        string path = Application.persistentDataPath + file + ".sav";
        File.Delete(path);
    }
}
