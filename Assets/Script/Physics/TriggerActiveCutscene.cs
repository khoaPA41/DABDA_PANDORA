using System;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerActiveCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneDirector;

    private BoxCollider triggerBoxCollider;

    private void Start()
    {
        triggerBoxCollider = GetComponent<BoxCollider>();
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cutsceneDirector.Play();
            triggerBoxCollider.enabled = false;
        }
    }
}
