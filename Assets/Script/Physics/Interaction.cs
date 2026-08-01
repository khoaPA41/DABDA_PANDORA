using System;
using System.Collections.Generic;
using System.Linq;
using Script.StateMachine.Player.Base;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class Interaction : MonoBehaviour
{
    [Header("Trigger Interaction")]
    [SerializeField] private List<TextMeshPro> gateTextList;
    [SerializeField] private SplineAnimate splineAnimate;

    [Header("Trigger Non Interaction")]
    [SerializeField] private PlayableDirector cutsceneDirector_1;
    
    [Header("Change Camera Script")]
    [SerializeField] private TriggerChangeCameraAndInput _triggerChangeCameraAndInput;


    private PlayerStateMachine _playerStateMachine;
    public event Action<string> ActiveButtonTriggerAction;
    public event Action <string> ActiveKeyTriggerAction;
    public event Action<GameObject> PickUpItemAction;
    public event Action EnterKeyAction;
    public event Action<bool> ActiveSplineStateAction;
    public event Action<int> ResetPlayerStateAction;
    private InputReader  _inputReader;

    public List<string> keyOwned;
    
    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        CheckAndActiveIfHaveKey();
    }

    public void CheckAndActiveIfHaveKey()
    {
        foreach (var key in keyOwned)
        {
            ActiveText(key);
        }
    }

    public void ActiveText(string name)
    {
        foreach (var text in gateTextList.Where(text => text.name == name))
        {
                text.gameObject.SetActive(true);
        }
    }
    
    public void ActiveSplineAnimate()
    {
        splineAnimate.Play();
    }

    public void ResetPlayerPositionAction(int transformIndex)
    {
        ResetPlayerStateAction?.Invoke(transformIndex);
    }
    
    public void ActiveSpline()
    {
        Debug.Log("Active Spline");
        _triggerChangeCameraAndInput?.ChangeSplineCamera(true);
        splineAnimate.Play();
        ActiveSplineStateAction?.Invoke(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathTrigger"))
        {
            Debug.Log("Death");
            _playerStateMachine.CallDeathAction();
        }
        
        if (other.CompareTag("Cutscene_1"))
        {
            splineAnimate.Pause();
            cutsceneDirector_1?.Play();
            ResetPlayerStateAction?.Invoke(0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Button_Obstacle_Trigger"))
        {
            if (_inputReader.IsInteract)
            {
                ActiveButtonTriggerAction?.Invoke(other.name);
                _inputReader.IsInteract = false;
            }
        }
        
        // if (other.CompareTag("Key_Obstacle_Trigger"))
        // {
        //     if (_inputReader.IsInteract)
        //     {
        //         ActiveKeyTriggerAction?.Invoke(other.name);
        //         _inputReader.IsInteract = false;
        //     }
        // }

        if (other.CompareTag("Item"))
        {
            Debug.Log(other.name);
            if (_inputReader.IsInteract)
            {
                PickUpItemAction?.Invoke(other.gameObject);
            }
        }
        
        if (other.CompareTag("GateLock"))
        {
            if (_inputReader.IsInteract)
            {
                EnterKeyAction?.Invoke();
                _inputReader.IsInteract = false;
            }
        }
        
        if (other.CompareTag("GateSpline"))
        {
            if (_inputReader.IsInteract)
            {
                splineAnimate.Play();
                ActiveSplineStateAction?.Invoke(false);
            }
        }

        if (other.CompareTag("ChangeSceneGate"))
        {
            other.GetComponent<TriggerChangeScene>().ChangeScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            transform.SetParent(null);
        }
    }
}
