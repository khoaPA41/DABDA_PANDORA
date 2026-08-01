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
        Respawn
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
        var saveData = SaveManager.Instance.currentSaveData;
        if (saveData is null)
        {
            Debug.Log("No save data loaded");
            return;
        }

        player.transform.position = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
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

    public void AutoSave()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player is null) return;

        var saveData = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            posX = player.transform.position.x,
            posY = player.transform.position.y,
            posZ = player.transform.position.z
        };
        
        SaveManager.Instance.SaveGame(saveData);
    }
}