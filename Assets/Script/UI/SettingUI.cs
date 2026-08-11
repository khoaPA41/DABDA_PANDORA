using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance { get; private set; }
    
    [Header("UI Elements")] [SerializeField]
    private GameObject settingsPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private GameObject graphicsPanel;

    [Header("Sound Settings")]
    [field: SerializeField] public SoundSettings sound { get; private set; }
    [field: SerializeField] public GraphicsSettings graphics { get; private set; }

    
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

    public void ExitSettings()
    {
        settingsPanel.SetActive(false);
    }
    
    private void InActiveAll()
    {
        soundPanel.SetActive(false);
        screenPanel.SetActive(false);
        graphicsPanel.SetActive(false);
    }
}