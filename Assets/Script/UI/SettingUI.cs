using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject exitWarningPanel;
    [SerializeField] private GameObject exitTab;

    [Header("Sound Settings")]
    [field: SerializeField] public SoundSettings sound { get; private set; }
    [field: SerializeField] public GraphicsSettingsUI graphics { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
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
        if (!scene.name.Contains("Start"))
        {
            exitTab.SetActive(true);
            return;
        }
        exitTab.SetActive(false);
    }

    public void ActiveSoundPanel()
    {
        InActiveAll();
        soundPanel.SetActive(true);
    }

    public void ActiveScreenPanel()
    {
        InActiveAll();
        screenPanel.SetActive(true);
    }

    public void ActiveGraphicsPanel()
    {
        InActiveAll();
        graphicsPanel.SetActive(true);
    }

    public void ActiveExitPanel()
    {
        InActiveAll();
        exitPanel.SetActive(true);
    }

    public void ActiveExitWarningPanel() => exitWarningPanel.SetActive(true);
    public void InactiveExitWarningPanel() => exitWarningPanel.SetActive(false);
    public void ExitSettings() => settingsPanel.SetActive(false);

    private void InActiveAll()
    {
        soundPanel.SetActive(false);
        screenPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        exitPanel.SetActive(false);
    }
}