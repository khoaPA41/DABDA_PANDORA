using System.IO;
using UnityEngine;

/// <summary>
/// Management read/write save file on storage (JSON)
/// </summary>


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {get; private set;}
    
    public  SaveData CurrentSaveData {get; private set;}

    private string savePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSaveData();
    }

    private bool HasSaveData()
    {
        return File.Exists(savePath);
    }

    public void CreatNewSaveData()
    {
        CurrentSaveData = new SaveData();
        Debug.Log("Creating Save Data");
    }

    public void SaveGame(SaveData saveData)
    {
        saveData.hasSaveData = true;
        var json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        CurrentSaveData = saveData;
        
        Debug.Log("Game Is Saved");
    }

    public SaveData LoadSaveData()
    {
        if (!HasSaveData())
        {
            Debug.Log("Dont Have Save Data");
            return null;
        }
        Debug.Log("Load Save Data");
        var json = File.ReadAllText(savePath);
        CurrentSaveData = JsonUtility.FromJson<SaveData>(json);
        return CurrentSaveData;
    }

    public void DeleteSaveData()
    {
        if (HasSaveData())
        {
            File.Delete(savePath);
        }

        CurrentSaveData = null;
        Debug.Log("Game Is Deleted");
    }
}
