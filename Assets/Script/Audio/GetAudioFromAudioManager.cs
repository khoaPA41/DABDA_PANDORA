using UnityEngine;

public class GetAudioFromAudioManager : MonoBehaviour
{ 
    public void PlayerBackgroundMusic(string musicName)
    {
        AudioManagers.Instance.PlayBackgroundMusic(musicName);
    }

    public void StopBackgroundMusic()
    {
        AudioManagers.Instance.StopBackgroundMusic();
    }
}
