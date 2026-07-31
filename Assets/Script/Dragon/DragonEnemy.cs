using System.Collections;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class DragonEnemy : MonoBehaviour
{
    [SerializeField] private float timeWaitToShoot;
    [SerializeField] private Transform projectile;

    private float health;
    private float speed;
    private Camera mainCamera;

    private PooledObject pooledObject;
    private Animator _animator;
    private bool isCanShoot;

    public void Init(DragonEnemyData data)
    {
        health = data.health;
        speed = data.speed;
        isCanShoot = data.isCanShoot;
    }

    private void Start()
    {
        pooledObject = GetComponent<PooledObject>();
        _animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        if (isCanShoot) StartCoroutine(ShootingCoroutine());
    }

    private void Update()
    {
        Move();
        CheckOutScreen(); // If out of screen => enemy die
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        // Debug.Log(health);
        if (health <= 0)
        {
            DragonDeath();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void CheckOutScreen()
    {
        if (mainCamera.WorldToScreenPoint(transform.position).y < -.1f)
        {
            pooledObject.Release(gameObject.name);
        }
    }

    private void DragonDeath()
    {
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject("Rock_Explosion", transform.position);
        pooledObject.Release(gameObject.name);
    }

    private void AutoShoot()
    {
        ActiveTriggerAttackAnimation();
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(BulletType.Enemy_Bullet.ToString(),
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
}