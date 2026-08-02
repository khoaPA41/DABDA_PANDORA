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
}
