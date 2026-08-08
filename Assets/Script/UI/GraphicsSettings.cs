using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Screen Element")] public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;
    public Toggle vsyncToggle;

    [Header("Graphics Element")] public TMP_Dropdown antiAliasingDropdown;
    public TMP_Dropdown textureQualityDropdown;
    public Toggle bloomToggle;
    public Toggle motionBlurToggle;
    public Toggle ambientOcclusionToggle;
    public TMP_Dropdown qualityPresetDropdown;
    public Toggle shadowsToggle;


    private bool isInitializing;

    private void Start()
    {
        isInitializing = true;
        ResolutionSetup();
        DisplayModeSetup();
        AntiAliasingSetup();
        QualityPresetSetup();
        TextureQualitySetup();
        
                
        LoadCurrentValuesToUI();
        BindListener();
        isInitializing = false;
    }

    private void ResolutionSetup()
    {
        resolutionDropdown.ClearOptions();
        var options = new List<string>();

        foreach (var resolution in GraphicsManager.Instance.AvailableResolutions)
        {
            options.Add($"{resolution.width}x{resolution.height}");
            
        }
        resolutionDropdown.AddOptions(options);
    }
    

    private void DisplayModeSetup()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string> { "Full Screen", "Borderless", "Windowed" });
    }

    private void AntiAliasingSetup()
    {
        antiAliasingDropdown.ClearOptions();
        antiAliasingDropdown.AddOptions(new List<string> { "None", "2x MSAA", "4x MSAA", "8x MSAA" });
    }

    private void QualityPresetSetup()
    {
        qualityPresetDropdown.ClearOptions();
        qualityPresetDropdown.AddOptions(new List<string>(QualitySettings.names));
    }

    private void TextureQualitySetup()
    {
        textureQualityDropdown.ClearOptions();
        textureQualityDropdown.AddOptions(new List<string> { "Ultra", "High", "Medium", "Low" });
    }

    private void LoadCurrentValuesToUI()
    {
        resolutionDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.resolutionIndex);
        displayModeDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.displayModeIndex);
        vsyncToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.vsync);
        qualityPresetDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.qualityPresentIndex);
        shadowsToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.shadow);
        antiAliasingDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.antiAliasingIndex);
        textureQualityDropdown.SetValueWithoutNotify(SaveManager.Instance.CurrentSaveData.textureQualityIndex);
        bloomToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.bloom);
        motionBlurToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.motionBlur);
        ambientOcclusionToggle.SetIsOnWithoutNotify(SaveManager.Instance.CurrentSaveData.ambientOcclusion);
    }

    private void BindListener()
    {
        resolutionDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicsManager.Instance.SetResolution(i);
            resolutionDropdown.SetValueWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        displayModeDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicsManager.Instance.SetDisplayMode(i);
            displayModeDropdown.SetValueWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        vsyncToggle.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicsManager.Instance.SetVsync(i);
            vsyncToggle.SetIsOnWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        shadowsToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicsManager.Instance.SetShadow(i);
            shadowsToggle.SetIsOnWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        qualityPresetDropdown.onValueChanged.AddListener(i =>
        {
            if(!isInitializing) GraphicsManager.Instance.SetQualityPresent(i);
            qualityPresetDropdown.SetValueWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        antiAliasingDropdown.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicsManager.Instance.SetAntiAliasing(i);
            antiAliasingDropdown.SetValueWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
        
        textureQualityDropdown.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicsManager.Instance.SetTextureQuality(i);
            textureQualityDropdown.SetValueWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
 
        bloomToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicsManager.Instance.SetBloom(i);
            bloomToggle.SetIsOnWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
 
        motionBlurToggle.onValueChanged.AddListener(i =>
        {
            Debug.Log(i);
            if (!isInitializing) GraphicsManager.Instance.SetMotionBlur(i);
            motionBlurToggle.SetIsOnWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
 
        ambientOcclusionToggle.onValueChanged.AddListener(i =>
        {
            if (!isInitializing) GraphicsManager.Instance.SetAmbientOcclusion(i);
            ambientOcclusionToggle.SetIsOnWithoutNotify(i);
            GameManager.Instance.AutoSave();
        });
    }
}
