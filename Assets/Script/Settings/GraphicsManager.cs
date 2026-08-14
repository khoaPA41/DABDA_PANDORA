using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using Bloom = UnityEngine.Rendering.Universal.Bloom;
using MotionBlur = UnityEngine.Rendering.Universal.MotionBlur;
using ShadowQuality = UnityEngine.ShadowQuality;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager Instance;

    [Header("Post Processing")] public Volume postProcessingVolume;
    [Header("Ambient Occlusion")] public ScriptableRendererFeature ambientOcclusionFeature;

    private Bloom _bloom;
    private MotionBlur _motionBlur;

    public List<Resolution> AvailableResolutions { get; private set; }

    public int ResolutionIndex { get; set; }
    public int DisplayModeIndex { get; set; }
    public bool Vsync { get; set; }
    public int QualityPresentIndex { get; set; }
    public bool Shadow { get; set; }
    public int AntiAliasingIndex { get; set; }
    public int TextureQualityIndex { get; set; }
    public bool BloomData { get; set; }
    public bool MotionBlurData { get; set; }
    public bool AmbientOcclusion { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildResolution();

        if (postProcessingVolume == null || postProcessingVolume.profile == null) return;
        postProcessingVolume.profile.TryGet<Bloom>(out _bloom);
        postProcessingVolume.profile.TryGet<MotionBlur>(out _motionBlur);

    }

    private void Start()
    {
        LoadApplyAll();
    }


    private void BuildResolution()
    {
        AvailableResolutions = new List<Resolution>();
        var seen = new HashSet<string>();

        /***************** Get current resolution first ************************/
        var currentResolution = Screen.currentResolution;
        var currentKey = $"{currentResolution.width}x{currentResolution.height}";
        seen.Add(currentKey);
        AvailableResolutions.Add(currentResolution);

        /*********************************************/
        foreach (var resolution in Screen.resolutions)
        {
            var key = $"{resolution.width}x{resolution.height}";
            if (seen.Contains(key)) continue;
            seen.Add(key);
            AvailableResolutions.Add(resolution);
        }

        AvailableResolutions.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));
    }

    /******************************** Apply each setting ********************************/

    public void SetResolution(int index)
    {
        if (index < 0 || index >= AvailableResolutions.Count) return;
        var resolution = AvailableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);

        // Save data
        ResolutionIndex = index;
    }

    public void SetDisplayMode(int index)
    {
        var mode = index switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => FullScreenMode.FullScreenWindow
        };
        Screen.fullScreenMode = mode;

        //Save data
        DisplayModeIndex = index;
    }

    public void SetVsync(bool active)
    {
        QualitySettings.vSyncCount = active ? 1 : 0;

        //Save data
        Vsync = active;
    }

    public void SetQualityPresent(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        //Save data
        QualityPresentIndex = index;
    }

    public void SetShadow(bool active)
    {
        var mainLight = RenderSettings.sun;
        if (mainLight == null) return;
        mainLight.shadows = active ? LightShadows.Soft : LightShadows.None;
        //Save data
        Shadow = active;
    }

    public void SetAntiAliasing(int index)
    {
        int[] msaaValue = { 0, 2, 4, 8 };

        var msaa = msaaValue[Mathf.Clamp(index, 0, msaaValue.Length - 1)];
        QualitySettings.antiAliasing = msaa;

        //Save data
        AntiAliasingIndex = index;
    }

    public void SetTextureQuality(int index)
    {
        QualitySettings.globalTextureMipmapLimit = index;

        //Save data
        TextureQualityIndex = index;
    }

    public void SetBloom(bool active)
    {
        if (_bloom != null) _bloom.active = active;

        //Save data
        BloomData = active;
    }

    public void SetMotionBlur(bool active)
    {
        if (_motionBlur != null) _motionBlur.active = active;
        //Save data
        MotionBlurData = active;
    }

    public void SetAmbientOcclusion(bool active)
    {
        if (ambientOcclusionFeature != null) ambientOcclusionFeature.SetActive(active); ;
        //Save data
        AmbientOcclusion = active;
    }

    public void LoadApplyAll()
    {
        if (SaveManager.Instance.CurrentSaveData is null)
        {
            SetResolution(16);
            SetDisplayMode(0);
            return;
        }
        SetResolution(SaveManager.Instance.CurrentSaveData.resolutionIndex);
        SetDisplayMode(SaveManager.Instance.CurrentSaveData.displayModeIndex);
        SetVsync(SaveManager.Instance.CurrentSaveData.vsync);
        SetQualityPresent(SaveManager.Instance.CurrentSaveData.qualityPresentIndex);
        SetShadow(SaveManager.Instance.CurrentSaveData.shadow);
        SetAntiAliasing(SaveManager.Instance.CurrentSaveData.antiAliasingIndex);
        SetTextureQuality(SaveManager.Instance.CurrentSaveData.textureQualityIndex);
        SetBloom(SaveManager.Instance.CurrentSaveData.bloom);
        SetMotionBlur(SaveManager.Instance.CurrentSaveData.motionBlur);
        SetAmbientOcclusion(SaveManager.Instance.CurrentSaveData.ambientOcclusion);
    }
}
