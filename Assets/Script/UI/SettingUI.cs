using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private GameObject settingsPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private GameObject graphicsPanel;


    private void Awake()
    {
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