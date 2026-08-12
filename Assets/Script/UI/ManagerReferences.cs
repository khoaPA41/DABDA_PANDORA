using UnityEngine;

public class ManagerReferences : MonoBehaviour
{
    /*Game Manager*/
    public void StartGame(string sceneName)
    {
        GameManager.Instance.StartGame(sceneName);
    }

    public void Continue()
    {
        GameManager.Instance.ContinueGame();
    }

    public void Exit()
    {
        GameManager.Instance.Exit();
    }

    public void Settings()
    {
        GameManager.Instance.ActiveSettingPanel();
    }

    /*Audio*/
    public void ButtonSound(string soundName)
    {
        AudioManagers.Instance.PlayButtonSound(soundName);
    }
}
