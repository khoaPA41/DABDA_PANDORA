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
    [SerializeField] float timeWaitToShoot;
    
    private Camera _mainCamera;
    private readonly BulletType _bulletType = BulletType.Normal_Bullet;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        inputReader = GetComponent<InputReader>();
        inputReader.SetCursor();
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
        ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(_bulletType.ToString(), projectile.position);
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