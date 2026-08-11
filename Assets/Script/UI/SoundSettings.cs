using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Settings UI")] 
    [SerializeField] private GameObject settings;
    
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;
    
    [Header("Slider Settings")] 
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;


    public float MasterVolume { get; set; } = .5f;
    public float BGMVolume { get; set; } = .5f;
    public float SfxVolume { get; set; } = .5f;
    public float UIVolume { get; set; } = .5f;
    
    private void Start()
    {
        UpdateSoundSettingFromSaveData();
        UpdateSoundSettings();
        SetMasterVolume(MasterVolume);
        SetBGMVolume(BGMVolume);
        SetSFXVolume(SfxVolume);
        SetUIVolume(UIVolume);
    }

    public void Setup()
    {
        UpdateSoundSettingFromSaveData();
        UpdateSoundSettings();
        SetMasterVolume(MasterVolume);
        SetBGMVolume(BGMVolume);
        SetSFXVolume(SfxVolume);
        SetUIVolume(UIVolume);
    }

    private void UpdateSoundSettingFromSaveData()
    {
        MasterVolume = SaveManager.Instance.CurrentSaveData.masterVolume;
        BGMVolume = SaveManager.Instance.CurrentSaveData.bgmVolume;
        SfxVolume = SaveManager.Instance.CurrentSaveData.sfxVolume;
        UIVolume = SaveManager.Instance.CurrentSaveData.uiVolume;
    }

    private void UpdateSoundSettings()
    {
        masterVolumeSlider.value = MasterVolume;
        bgmVolumeSlider.value = BGMVolume;
        sfxVolumeSlider.value = SfxVolume;
        uiVolumeSlider.value = UIVolume;
    }
    
    private void SetMasterVolume(float volume)
    {
        var safeVolume = Mathf.Clamp(volume, 0.0001f, 1.0f);
        mixer.SetFloat("Master", Mathf.Log10(safeVolume) * 20f);

        if (settings.activeInHierarchy)
        {
            MasterVolume = masterVolumeSlider.value;
        }
    }

    private void SetBGMVolume(float volume)
    {
        var safeVolume = Mathf.Clamp(volume, 0.0001f, 1.0f);

        mixer.SetFloat("BGM", Mathf.Log10(safeVolume) * 20f);
        if (settings.activeInHierarchy)
        {
            BGMVolume = bgmVolumeSlider.value;
        }
    }
    
    private void SetSFXVolume(float volume)
    {
        var safeVolume = Mathf.Clamp(volume, 0.0001f, 1.0f);

        mixer.SetFloat("SFX", Mathf.Log10(safeVolume) * 20f);
        if (settings.activeInHierarchy)
        {
            SfxVolume = sfxVolumeSlider.value;
        }
    }
    
    private void SetUIVolume(float volume)
    {
        var safeVolume = Mathf.Clamp(volume, 0.0001f, 1.0f);

        mixer.SetFloat("UI", Mathf.Log10(safeVolume) * 20f);
        if (settings.activeInHierarchy)
        {
            UIVolume = uiVolumeSlider.value;
        }
    }
    
}
