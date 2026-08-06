using System;
using UnityEngine;

public class PlayUISound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource =  GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
