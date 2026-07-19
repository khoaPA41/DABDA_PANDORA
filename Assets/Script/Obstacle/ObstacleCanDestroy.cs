using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class ObstacleCanDestroy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int health;
    [SerializeField] private ObjectPooling _destroyedParticleSystem;

    public event Action OnDestroyedAction;
    private void Start()
    {
        OnDestroyedAction += Destroyed;
    }

    private void Update()
    {
        // if (health <= 0)
        // {
        //     Destroyed();
        // }
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
        var particleObject = _destroyedParticleSystem.GetPooledObject("Rock_Explosion", transform.position);
        particleObject.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
