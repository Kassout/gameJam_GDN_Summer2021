using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Class <c>PersistantData</c> is used to manager persistant game data and player progress save.
/// </summary>
[Serializable]
public class PersistantData
{
    /// <summary>
    /// Static variable <c>PersistantData</c> representing the instance of the class. 
    /// </summary>
    public static PersistantData Instance { get; private set; }
    
    /// <summary>
    /// Static variable <c>Level</c> representing the level 
    /// </summary>
    public int level = 1;
    
    /// <summary>
    /// This method is used to save the different attributes class values in a json file.
    /// </summary>
    public void SavePersistantData()
    {
        PersistantData data = new PersistantData();

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    /// <summary>
    /// This method is used to load the different attributes class values from a json file.
    /// </summary>
    public void LoadPersistantData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PersistantData data = JsonUtility.FromJson<PersistantData>(json);

            level = data.level;
        }
    }
}
