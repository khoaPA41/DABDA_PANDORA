using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class will control the NEW/ CONTINUE/ SAVE/ RESPAWN
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum ReasonLoadScene
    {
        New,
        Continue,
        Respawn,
        NextLevel
    }

    private Vector3 checkPointPosition;
    private ReasonLoadScene _loadSceneReason = ReasonLoadScene.New;

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

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start") return;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;

        switch (_loadSceneReason)
        {
            case ReasonLoadScene.New:
                break;
            case ReasonLoadScene.Continue:
                ApplySaveData(player);
                break;
            case ReasonLoadScene.Respawn:
                ApplySaveData(player);
                break;
            case ReasonLoadScene.NextLevel:
                AutoSave();
                break;
        }
    }

    public void StartGame(string sceneName)
    {
        SaveManager.Instance.CreatNewSaveData();
        _loadSceneReason = ReasonLoadScene.New;
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueGame()
    {
        var saveData = SaveManager.Instance.LoadSaveData();
        if (saveData is null)
        {
            Debug.Log("No save data loaded");
            StartGame("Denial");
            return;
        }

        _loadSceneReason = ReasonLoadScene.Continue;
        checkPointPosition = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
        SceneManager.LoadScene(saveData.sceneName);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ApplySaveData(GameObject player)
    {
        var saveData = SaveManager.Instance.CurrentSaveData;
        if (saveData is null)
        {
            Debug.Log("No save data loaded");
            return;
        }
        var map = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManagers>();
        map.isActiveObstacleTrigger_I = saveData.isActiveObstacle_I;
        player.transform.position = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
        player.GetComponent<Interaction>().keyOwned = saveData.keyName;
        player.GetComponent<TriggerChangeCameraAndInput>().SetSaveDataCamera(saveData.currentCameraName, saveData.previousCameraName);
    }

    public void SetCheckPoint(Vector3 pos)
    {
        checkPointPosition = pos;
    }

    public void ReturnTile()
    {
        SceneManager.LoadScene("Start");
    }

    public void ReturnCheckpoint()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;
        _loadSceneReason = ReasonLoadScene.Respawn;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        _loadSceneReason = ReasonLoadScene.NextLevel;
    }

    public void AutoSave()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var map = GameObject.FindGameObjectWithTag("MapManager");
        var mapManager = map?.GetComponent<MapManagers>();
        if (player is null) return;
        
        var previousCameraObject = player.GetComponent<TriggerChangeCameraAndInput>().PreviousCamera;
        var currentCameraObject = player.GetComponent<TriggerChangeCameraAndInput>().CurrentCamera;
        var keyOwned = player.GetComponent<Interaction>().keyOwned;

        Debug.Log($"SaveManager.Instance: {SaveManager.Instance}");
        Debug.Log($"currentSaveData: {SaveManager.Instance.CurrentSaveData}");
        Debug.Log($"map: {mapManager}");
        var isActiveObstacleTrigger_I = mapManager is null ? SaveManager.Instance.CurrentSaveData.isActiveObstacle_I : mapManager.isActiveObstacleTrigger_I;
        
        var saveData = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            posX = player.transform.position.x,
            posY = player.transform.position.y,
            posZ = player.transform.position.z,
            previousCameraName =  previousCameraObject.name,
            currentCameraName = currentCameraObject.name,
            keyName = new List<string>(keyOwned),
            isActiveObstacle_I = isActiveObstacleTrigger_I
        };
        
        SaveManager.Instance.SaveGame(saveData);
    }
}