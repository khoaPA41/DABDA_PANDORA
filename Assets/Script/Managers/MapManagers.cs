using System.Collections.Generic;
using UnityEngine;

public class MapManagers : MonoBehaviour
{
    [Header("Map I Item")]
    [SerializeField] List<GameObject> MapObjects;
    [SerializeField] GameObject ObstacleObjects;
    
    public bool isActiveObstacleTrigger_I { get; set; }
    
    private void Start()
    {
        CheckActiveObstacle();
        CheckKeyCollected();
    }

    private void CheckActiveObstacle()
    {
        ObstacleObjects.SetActive(!isActiveObstacleTrigger_I);
    }
    

    private void CheckKeyCollected()
    {
        if (SaveManager.Instance.CurrentSaveData.keyName is null) return;
        foreach (var itemKey in SaveManager.Instance.CurrentSaveData.keyName)
        {
            foreach (var itemCollected in MapObjects)
            {
                if (itemCollected.name == itemKey)
                {
                    itemCollected.SetActive(false);
                }
            }
        }
    }
}
