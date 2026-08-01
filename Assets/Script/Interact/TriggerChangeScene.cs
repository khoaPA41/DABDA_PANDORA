using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerChangeScene : MonoBehaviour
{
    [field: SerializeField] public string sceneName {get; private set;}


    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
