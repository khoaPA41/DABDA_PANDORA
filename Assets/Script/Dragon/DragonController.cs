using System.Collections;
using UnityEngine;

public enum BulletType
{
    Normal_Bullet,
    Enemy_Bullet
}

public class DragonController : MonoBehaviour
{
    [Header("Input Script")] [SerializeField]
    private InputReader inputReader;
    
    [Header("Movement")] [SerializeField] private float speed;
    [SerializeField] private float viewPointMargin;
    
    [Header("Projectile")] [SerializeField] Transform projectile;
    [SerializeField] Transform subProjectile_I;
    [SerializeField] Transform subProjectile_II;

    [SerializeField] float timeWaitToShoot;
    
    private Camera _mainCamera;
    private readonly BulletType _bulletType = BulletType.Normal_Bullet;
    
    
    public bool isBuffProjectile;
    private void Start()
    {
        _mainCamera = Camera.main;
        inputReader = GetComponent<InputReader>();
        inputReader.SetCursor();
    }

    private void OnEnable()
    {
        if (ObjectPoolManager.Instance is null) return;
        StartCoroutine(ShootingCoroutine());
    }

    private void Update()
    {
        Move();
        ClampPosition();
    }

    private void Move()
    {
        var direction = new Vector3(inputReader.Movement.x, 0f, inputReader.Movement.y);

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void ClampPosition()
    {
        var viewPointConvert = _mainCamera.WorldToViewportPoint(transform.position);
        viewPointConvert.x = Mathf.Clamp(viewPointConvert.x, viewPointMargin, 1f - viewPointMargin);
        viewPointConvert.y = Mathf.Clamp(viewPointConvert.y, viewPointMargin, 1f - viewPointMargin);
        transform.position = _mainCamera.ViewportToWorldPoint(viewPointConvert);
    }

    private void AutoShoot()
    {
        
        if (isBuffProjectile)
        {
            AutoShootBuffProjectile();
        }
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(_bulletType.ToString(), projectile.position);
    }

    private void AutoShootBuffProjectile()
    {
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(_bulletType.ToString(), subProjectile_I.position);
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(_bulletType.ToString(), subProjectile_II.position);
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            AutoShoot();
            yield return new WaitForSecondsRealtime(timeWaitToShoot);
        }
    }
}