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
    public string sceneName;
    public Vector3 position;
    public CameraStatus cameraStatus;
    public List<string> keyName;
}
