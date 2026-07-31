using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public enum BuffType
{
    Healing,
    Projectile,
    Immortal
}

public class DragonBuff : MonoBehaviour
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float speed;

    private PooledObject _pooledObject;
    private Camera _mainCamera;


    private void Start()
    {
        _pooledObject = GetComponent<PooledObject>();
        _mainCamera =  Camera.main;
    }

    private void Update()
    {
        CheckPositionToRelease();
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    private void CheckPositionToRelease()
    {
        if (_mainCamera.WorldToViewportPoint(transform.position).y < -.1f)
        {
            _pooledObject.Release(gameObject.name);
        }
    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<DragonBuffHandler>().ActiveBaseOnBuff(buffType);
            _pooledObject.Release(gameObject.name);
        }
    }
}
