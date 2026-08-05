using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagers : MonoBehaviour
{
    public static AudioManagers Instance;
    
    public List<AudioClip> backgroundMusicList;
    private AudioSource audioSource;
    
    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic("Start");
    }

    public void PlayBackgroundMusic(string musicName)
    {
        foreach (var clip in backgroundMusicList.Where(clip => clip.name == musicName))
        {
            audioSource.clip = clip;
        }
        audioSource.Play();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource?.Stop();
    }

    public void StopBackgroundMusic()
    {
        audioSource?.Stop();
    }
    
    
    // /*Player Audio*/
    // public void 
}
