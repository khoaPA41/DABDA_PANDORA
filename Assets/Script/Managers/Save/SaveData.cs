using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This all data need to save in once playing
/// This class need to mark Serializable to JsonUtility convert JSON
/// </summary>
[Serializable]
public class SaveData
{
    public bool hasSaveData;
    public string sceneName;
    public float posX;
    public float posY;
    public float posZ;
    public string previousCameraName;
    public string currentCameraName;
    public List<string> keyName = new List<string>();
    public bool isActiveObstacle_I;

    /*Settings*/
    //Graphics
    public int resolutionIndex = 16;
    public int displayModeIndex;
    public bool vsync;
    public int qualityPresentIndex;
    public bool shadow;
    public int antiAliasingIndex;
    public int textureQualityIndex;
    public bool bloom;
    public bool motionBlur;
    public bool ambientOcclusion;

    //Sound
    public float masterVolume = .5f;
    public float bgmVolume = .5f;
    public float sfxVolume = .5f;
    public float uiVolume = .5f;
}