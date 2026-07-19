using System;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")] [SerializeField]
    private float speed;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider boxCollider;
    
    public Vector3 direction;
    private PooledObject _pooledObject;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        _pooledObject = GetComponent<PooledObject>();
    }
    
    void Update()
    {
        BulletMovement();
        BulletRaycast();
    }

    private void BulletMovement()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void BulletRaycast()
    {
        var exactCenter = boxCollider.transform.TransformPoint(boxCollider.center);
        var realSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);
        if (Physics.BoxCast(exactCenter, realSize * .5f, boxCollider.transform.forward, out var hit, boxCollider.transform.rotation, Mathf.Infinity,obstacleLayer))
        {
            var obstacle = hit.collider.GetComponent<ObstacleCanDestroy>();
            obstacle.SubtractHealth(1);
            _pooledObject.Release("Bullet");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        var exactCenter = boxCollider.transform.TransformPoint(boxCollider.center); // Calculate center to world space
        var realSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale); // Calculate real size with lossyScale

        Gizmos.matrix = Matrix4x4.TRS(exactCenter, boxCollider.transform.rotation, Vector3.one);
        
        Gizmos.DrawWireCube(Vector3.zero, realSize);
    }
}
