using Script.Design_Pattern.Object_Pooling;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerAudio))]

[RequireComponent(typeof(InputReader))]
public class Shooting : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private Canvas Canvas;
    [SerializeField] private Image crosshairImage;

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
    private RectTransform crosshair;
    private PlayerAudio _playerAudio;
    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
        _playerAudio = GetComponent<PlayerAudio>();
        _mainCamera = Camera.main;
        crosshair = crosshairImage.rectTransform;
    }

    void Update()
    {
        if (!isCanShoot) return;
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
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            // target.position = hitInfo.;
        }
    }

    private void Shoot()
    {
        if (!_inputReader.IsAttack) return;
        _playerAudio.ThrowBulletSound();
        objectPooling.GetPooledObject("Bullet", projectile.position).GetComponent<Bullet>().Direction = target;
        _inputReader.IsAttack = false;
    }

    public void ActiveCrosshair(bool active)
    {
        crosshairImage.gameObject.SetActive(active);
        isCanShoot = active;
    }
}
