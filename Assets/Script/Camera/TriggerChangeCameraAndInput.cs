using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangeCameraAndInput : MonoBehaviour
{
    [Header("Camera Object")] [SerializeField] 
    private List<GameObject> mainCameraList;
    // [SerializeField] private GameObject camera2D_I;
    // [SerializeField] private GameObject camera2D_II;
    //
    // [SerializeField] private GameObject camera3D;
    [SerializeField] private GameObject cameraTargetGate;
    [SerializeField] private GameObject splineCamera;

    public bool IsChangeInputState = true;

    public event Action<CameraStatus> ChangeCameraStateAction;

    private void OnEnable()
    {
        ChangeCameraStateAction += ChangeCamera;
    }

    private void OnDisable()
    {
        ChangeCameraStateAction -= ChangeCamera;
    }

    private void ChangeCamera(CameraStatus cameraStatus)
    {
        IsChangeInputState = cameraStatus.isChangeInputState;
        mainCameraList[mainCameraList.IndexOf(mainCameraList.Find(camera  => camera.name == cameraStatus.name))].SetActive(true);
        ResetCamera(cameraStatus.name);
        // camera2D.SetActive(!Is3DState);
        // camera3D.SetActive(Is3DState);
    }

    private void ResetCamera(string name)
    {
        foreach (var camera in mainCameraList)
        {
            if (camera.name == name) continue;
            camera.SetActive(false);
        }
    }

    public void ChangeCameraTargetGateCoroutine()
    {
        StartCoroutine(ResetCameraCoroutine(ChangeCameraTargetGate));
    }

    public void ChangeSplineCamera(bool isActive)
    {
        splineCamera.SetActive(isActive);
    }

    private void ChangeCameraTargetGate()
    {
        cameraTargetGate.SetActive(!cameraTargetGate.activeInHierarchy);
    }

    private IEnumerator ResetCameraCoroutine(Action action)
    {
        action?.Invoke();
        yield return new WaitForSeconds(3f);
        action?.Invoke();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChangeEnvironmentState"))
        {
            ChangeCameraStateAction?.Invoke(other.gameObject.GetComponent<CameraStatus>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ChangeEnvironmentState")) return;

        other.GetComponent<BoxCollider>().isTrigger = false;
    }
}
