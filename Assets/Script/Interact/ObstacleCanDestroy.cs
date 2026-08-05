using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class ObstacleCanDestroy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int health;
    [SerializeField] private ObjectPooling destroyedParticleSystem;
    [SerializeField] private AudioClip explosion;

    public event Action OnDestroyedAction;
    private void Start()
    {
        OnDestroyedAction += Destroyed;
    }

    public void SubtractHealth(int amount)
    {
        health  = Mathf.Max(health - amount, 0);
        if (health <= 0)
        {
            OnDestroyedAction?.Invoke();
        }
    }
    
    private void Destroyed()
    {
        var particleObject = destroyedParticleSystem.GetPooledObject("Rock_Explosion", transform.position);
        var sound = destroyedParticleSystem.GetPooledObject("Sound_Effect", transform.position).GetComponent<AudioSource>();
        sound.clip = explosion;
        sound.Play();
        particleObject.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
