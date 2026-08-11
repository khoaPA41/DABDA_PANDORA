using System;
using UnityEngine;

public class TriggerPlayMusic : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private GetAudioFromAudioManager getAudioFromAudioManager;
    
    private BoxCollider boxCollider;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        getAudioFromAudioManager.PlayerBackgroundMusic("Final_Music");
        boxCollider.enabled = false;
    }
}
