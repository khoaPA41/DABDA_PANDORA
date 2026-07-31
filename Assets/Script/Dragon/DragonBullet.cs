using System;
using Script.Design_Pattern.Object_Pooling;
using Unity.VisualScripting;
using UnityEngine;

public class DragonBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private bool isPlayer;

    private PooledObject _pooledObject;
    private Vector3 _previousPosition;
    private Vector3 castLength;
    private Camera _mainCamera;
    private float yViewPoint;
    
    private void Start()
    {
        _pooledObject = GetComponent<PooledObject>();
        _mainCamera =  Camera.main;
    }
    
    private void Update()
    {
        MoveAndCheckCast();
        CheckReleasePosition();
    }


    private void MoveAndCheckCast()
    {
        _previousPosition =  transform.position;
        var direction = isPlayer ? Vector3.forward : -Vector3.forward;
        transform.Translate(direction * speed * Time.deltaTime);
        castLength = transform.position - _previousPosition;

        if (Physics.Raycast(_previousPosition, castLength.normalized, out RaycastHit hit, castLength.magnitude, enemyMask))
        {
            if (hit.transform.TryGetComponent(out DragonEnemy health))
            {
                health.TakeDamage(damage);
                _pooledObject.Release(gameObject.name);
            }

            if (hit.transform.TryGetComponent(out DragonHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }

    private void CheckReleasePosition()
    {
        if (isPlayer)
        {
            if (_mainCamera.WorldToViewportPoint(transform.position).y > 1.1f)
            {
                _pooledObject.Release(gameObject.name);
            }
        }
        else
        {
            if (_mainCamera.WorldToViewportPoint(transform.position).y < -.1f)
            {
                _pooledObject.Release(gameObject.name);
            }
        }
    }
}
