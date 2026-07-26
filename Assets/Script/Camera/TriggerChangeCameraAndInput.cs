using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private GameObject previousCamera;

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

        var currentCameraName = CheckCurrentCameraActive(cameraStatus) ? cameraStatus.cameraName : previousCamera.name;
        
        ActiveCamera(currentCameraName);
        
        ResetCamera(currentCameraName);
    }


    private bool CheckCurrentCameraActive(CameraStatus cameraStatus)
    {
        foreach (var camera in mainCameraList)
        {
            if (camera.activeInHierarchy && camera.name != cameraStatus.cameraName)
            {
                previousCamera = camera;
                Debug.Log(previousCamera.name);
                return true;
            }
        }

        return false;
    }

    private void ActiveCamera(string cameraName)
    {
        mainCameraList[mainCameraList.IndexOf(mainCameraList.Find(camera => camera.name == cameraName))]
            .SetActive(true);
    }
    
    private void ResetCamera(string name)
    {
        foreach (var camera in mainCameraList)
        {
            if (camera.name == name) continue;
            camera.SetActive(false);
        }
    }

    public void ChangeCameraTargetGateCoroutine(float time)
    {
        StartCoroutine(ResetCameraCoroutine(ChangeCameraTargetGate, time));
    }

    public void ChangeSplineCamera(bool isActive)
    {
        splineCamera.SetActive(isActive);
    }

    private void ChangeCameraTargetGate()
    {
        cameraTargetGate.SetActive(!cameraTargetGate.activeInHierarchy);
    }

    private static IEnumerator ResetCameraCoroutine(Action action, float time)
    {
        action?.Invoke();
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChangeEnvironmentState"))
        {
            ChangeCameraStateAction?.Invoke(other.gameObject.GetComponent<CameraStatus>());
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (!other.CompareTag("ChangeEnvironmentState")) return;
    // }
}