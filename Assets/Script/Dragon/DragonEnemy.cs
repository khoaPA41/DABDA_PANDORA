using System.Collections;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class DragonEnemy : MonoBehaviour
{
    [SerializeField] private float timeWaitToShoot;
    [SerializeField] private Transform projectile;

    private float _health;
    private float _speed;
    private Camera _mainCamera;
    private PooledObject _pooledObject;
    private Animator _animator;
    private bool _isCanShoot;

    public void Init(DragonEnemyData data)
    {
        _health = data.health;
        _speed = data.speed;
        _isCanShoot = data.isCanShoot;
    }

    private void Start()
    {
        _pooledObject = GetComponent<PooledObject>();
        _animator = GetComponent<Animator>();
        if (_isCanShoot) StartCoroutine(ShootingCoroutine());
    }

    private void OnEnable()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        _mainCamera ??= Camera.main;
        if (_mainCamera is null) return;

        Move();
        CheckOutScreen(); // If out of screen => enemy die
    }


    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DragonDeath();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void CheckOutScreen()
    {
        if (_mainCamera.WorldToScreenPoint(transform.position).y < -.1f)
        {
            _pooledObject.Release(gameObject.name);
        }
    }

    private void DragonDeath()
    {
        ObjectPooling.Instance.GetPooledObject("Rock_Explosion", transform.position);
        _pooledObject.Release(gameObject.name);
    }

    private void AutoShoot()
    {
        ActiveTriggerAttackAnimation();
        ObjectPooling.Instance.GetPooledObject(nameof(BulletType.Enemy_Bullet),
        projectile.position);
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            AutoShoot();
            yield return new WaitForSecondsRealtime(timeWaitToShoot);
        }
    }

    private void ActiveTriggerAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isCanShoot && other.CompareTag("Player"))
        {
            other.GetComponent<DragonHealth>().TakeDamage(100f);
        }
    }
}