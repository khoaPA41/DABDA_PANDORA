using System.IO;
using UnityEngine;

/// <summary>
/// Management read/write save file on storage (JSON)
/// </summary>


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {get; private set;}
    
    public  SaveData currentSaveData {get; private set;}

    public string savePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private bool HasSaveData()
    {
        return File.Exists(savePath);
    }

    public void CreatNewSaveData()
    {
        currentSaveData = new SaveData();
        Debug.Log("Creating Save Data");
    }

    public void SaveGame(SaveData saveData)
    {
        saveData.hasSaveData = true;
        var json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        currentSaveData = saveData;
        
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
        currentSaveData = JsonUtility.FromJson<SaveData>(json);
        return currentSaveData;
    }

    public void DeleteSaveData()
    {
        if (HasSaveData())
        {
            File.Delete(savePath);
        }

        currentSaveData = null;
        Debug.Log("Game Is Deleted");
    }
}
