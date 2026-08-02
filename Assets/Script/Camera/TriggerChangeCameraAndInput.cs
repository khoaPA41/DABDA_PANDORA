using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerChangeCameraAndInput : MonoBehaviour
{
    [Header("Camera Object")] [SerializeField]
    private List<GameObject> mainCameraList;
    [SerializeField] private GameObject cameraTargetGate;
    [SerializeField] private GameObject splineCamera;

    public bool IsChangeInputState = true;

    public event Action<CameraStatus> ChangeCameraStateAction;

    public GameObject PreviousCamera { get; private set; }
    public GameObject CurrentCamera { get; private set; }


    private void Awake()
    {
        SetSaveDataCamera(SaveManager.Instance.CurrentSaveData.currentCameraName, SaveManager.Instance.CurrentSaveData.previousCameraName);
    }
    
    private void OnEnable()
    {
        ChangeCameraStateAction += ChangeCamera;
    }

    private void OnDisable()
    {
        ChangeCameraStateAction -= ChangeCamera;
    }
    
    public void SetSaveDataCamera(string currentCameraName, string previousCameraName)
    {
        if (string.IsNullOrEmpty(currentCameraName))
        {
            foreach (var camera in mainCameraList)
            {
                PreviousCamera = camera;
                CurrentCamera = camera;
            }
            return;
        }

        ActiveCamera(currentCameraName);
        ResetCamera(currentCameraName);
        foreach (var camera in mainCameraList.Where(camera => camera.name == previousCameraName))
        {
            PreviousCamera = camera;
        }
        Debug.Log(PreviousCamera.name);
        Debug.Log(CurrentCamera.name);
    }

    private void ChangeCamera(CameraStatus cameraStatus)
    {
        IsChangeInputState = cameraStatus.isChangeInputState;

        var currentCameraName = CheckCurrentCameraActive(cameraStatus) ? cameraStatus.cameraName : PreviousCamera.name;
        
        ActiveCamera(currentCameraName);
        ResetCamera(currentCameraName);
        Debug.Log(PreviousCamera.name);
        Debug.Log(CurrentCamera.name);
    }


    private bool CheckCurrentCameraActive(CameraStatus cameraStatus)
    {
        foreach (var camera in mainCameraList)
        {
            if (camera.activeInHierarchy && camera.name != cameraStatus.cameraName)
            {
                PreviousCamera = camera;
                return true;
            }
        }

        return false;
    }

    private void ActiveCamera(string cameraName)
    {
        CurrentCamera = mainCameraList[mainCameraList.IndexOf(mainCameraList.Find(camera => camera.name == cameraName))];
        CurrentCamera.SetActive(true);
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
}