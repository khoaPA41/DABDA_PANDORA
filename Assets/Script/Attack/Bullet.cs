using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PooledObject))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField]
    private float speed;
    [SerializeField] private float timeToReturnToPool;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider boxCollider;

    public Vector3 Direction { get; set; }
    private PooledObject _pooledObject;
    private float countTime;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        _pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        countTime = 0f; // Reset Count Time
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    void Update()
    {
        if (ReturnToPool()) return;
        BulletMovement();
        if (BulletRaycast()) return;
    }

    private void BulletMovement()
    {
        rb.MovePosition(rb.position + speed * Time.deltaTime * Direction);
    }

    private bool ReturnToPool()
    {
        countTime += Time.deltaTime;
        if (countTime >= timeToReturnToPool)
        {
            _pooledObject.Release("Bullet");
            return true;
        }
        return false;
    }

    private bool BulletRaycast()
    {
        var exactCenter = boxCollider.transform.TransformPoint(boxCollider.center);
        var realSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);
        var castDistance = speed * Time.deltaTime;
        if (Physics.BoxCast(exactCenter, realSize * .5f, Direction.normalized, out var hit, boxCollider.transform.rotation, castDistance, obstacleLayer))
        {
            var obstacle = hit.collider.GetComponent<ObstacleCanDestroy>();
            obstacle.SubtractHealth(1);
            _pooledObject.Release("Bullet");
            return true;
        }
        return false;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;

    //     var exactCenter = boxCollider.transform.TransformPoint(boxCollider.center); // Calculate center to world space
    //     var realSize = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale); // Calculate real size with lossyScale

    //     Gizmos.matrix = Matrix4x4.TRS(exactCenter, boxCollider.transform.rotation, Vector3.one);

    //     Gizmos.DrawWireCube(Vector3.zero, realSize);
    // }
}
