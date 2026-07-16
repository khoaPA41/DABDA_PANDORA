using System;
using UnityEngine;

public class TriggerChangeCameraAndInput : MonoBehaviour
{
    [Header("Camera Object")]
    [SerializeField] private GameObject camera2D;
    [SerializeField] private GameObject camera3D;

    public bool Is3DState { get; private set; } = true;

    public event Action ChangeCameraStateAction;

    private void OnEnable()
    {
        ChangeCameraStateAction += ChangeCamera;
    }

    private void OnDisable()
    {
        ChangeCameraStateAction -= ChangeCamera;
    }

    private void ChangeCamera()
    {
        Is3DState = !Is3DState;
        camera2D.SetActive(!Is3DState);
        camera3D.SetActive(Is3DState);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChangeEnvironmentState"))
        {
            ChangeCameraStateAction?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ChangeEnvironmentState")) return;

        other.GetComponent<BoxCollider>().isTrigger = false;
    }
}
