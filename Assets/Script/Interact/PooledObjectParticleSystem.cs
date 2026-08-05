using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class PooledObjectParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private PooledObject _pooledObject;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        if (_particleSystem is null) return;
        _particleSystem.Clear();
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _pooledObject.Release("Rock_Explosion");
    }
}