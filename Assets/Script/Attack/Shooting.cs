using Script.Design_Pattern.Object_Pooling;
using Unity.Mathematics;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas Canvas;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float sensitivity;
    [SerializeField] private float crosshairPadding;
    
    [Header("Bullet Pool")]
    [SerializeField] private ObjectPooling objectPooling;
    
    [Header("Projectile")]
    [SerializeField] private Transform projectile;
    
    public bool isCanShoot { get; set; }
    private InputReader _inputReader;
    private Camera _mainCamera;
    private Vector3 target;
    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // if (!isCanShoot) return;
        var newRectPos = _inputReader.Look * (sensitivity * Time.deltaTime);
        crosshair.anchoredPosition += newRectPos;
        ClampCrosshairPosition();
        AimRaycast();
        Shoot();
    }


    private void ClampCrosshairPosition()
    {
        var canvasRect = Canvas.GetComponent<RectTransform>().rect;
        var halfWidth = canvasRect.width / 2;
        var halfHeight = canvasRect.height / 2;
        // var halfHeight = canvasRect.height / 2 * Canvas.scaleFactor;

        var currentCrosshairPos = crosshair.anchoredPosition;
        currentCrosshairPos.x = Mathf.Clamp(currentCrosshairPos.x, -halfWidth + crosshairPadding, halfWidth - crosshairPadding);
        currentCrosshairPos.y = Mathf.Clamp(currentCrosshairPos.y, -halfHeight + crosshairPadding, halfHeight - crosshairPadding);
        crosshair.anchoredPosition = currentCrosshairPos;
    }

    private void AimRaycast()
    {
        Ray ray = _mainCamera.ScreenPointToRay(crosshair.position);
        target = ray.direction;
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity))
        {
            // target.position = hitInfo.;
        }
    }

    private void Shoot()
    {
        if (!_inputReader.IsAttack) return;
        objectPooling.GetPooledObject("Bullet", projectile.position).GetComponent<Bullet>().direction = target;
        _inputReader.IsAttack = false;
    }
}
