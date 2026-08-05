using UnityEngine;

public class GetAudioFromAudioManager : MonoBehaviour
{ 
    
    /*Player background music - loop*/
    public void PlayerBackgroundMusic(string musicName)
    {
        AudioManagers.Instance.PlayBackgroundMusic(musicName);
    }

    public void StopBackgroundMusic()
    {
        AudioManagers.Instance.StopBackgroundMusic();
    }
    
    /*Player background music - no loop*/

    public void PlayerNoLoopBackgroundMusic(string musicName)
    {
        AudioManagers.Instance.PlayNoLoopMusic(musicName);
    }
}
