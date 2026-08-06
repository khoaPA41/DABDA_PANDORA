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
    [Header("Trigger Interaction")] [SerializeField]
    private List<TextMeshPro> gateTextList;
    [SerializeField] private SplineAnimate splineAnimate;

    [Header("Trigger Non Interaction")] [SerializeField]
    private PlayableDirector cutsceneDirector_1;

    [Header("Change Camera Script")] [SerializeField]
    private TriggerChangeCameraAndInput _triggerChangeCameraAndInput;

    private PlayerStateMachine _playerStateMachine;
    public event Action<string> ActiveButtonTriggerAction;
    public event Action<GameObject> PickUpItemAction;
    public event Action EnterKeyAction;
    public event Action<bool> ActiveSplineStateAction;
    public event Action<int> ResetPlayerStateAction;
    private InputReader _inputReader;
    
    private string itemKey;

    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        CheckAndActiveIfHaveKey();
    }

    private void CheckAndActiveIfHaveKey()
    {
        if (gateTextList.Count == 0 || gateTextList[0] is null) return;
        if (GameManager.Instance.keyOwnedList.Count == 0) return;
        foreach (var key in GameManager.Instance.keyOwnedList)
        {
            ActiveText(key);
        }
    }

    public void ActiveText(string itemName)
    {
        foreach (var text in gateTextList.Where(text => text.name == itemName))
        {
            text.gameObject.SetActive(true);
        }
        GameManager.Instance.AutoSave();
    }

    public void ResetPlayerPositionAction(int transformIndex)
    {
        ResetPlayerStateAction?.Invoke(transformIndex);
    }

    public void ActiveSpline()
    {
        _triggerChangeCameraAndInput?.ChangeSplineCamera(true);
        splineAnimate.Play();
        ActiveSplineStateAction?.Invoke(true);
    }

    public void GetKeyName(string key)
    {
        itemKey = key;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathTrigger"))
        {
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
            if (_inputReader.IsInteract)
            {
                GetKeyName(other.name);
                PickUpItemAction?.Invoke(other.gameObject);
                _inputReader.IsInteract = false;
            }
        }

        if (other.CompareTag("GateLock"))
        {
            if (_inputReader.IsInteract)
            {
                GameManager.Instance.AddKey(itemKey);
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
                other.gameObject.SetActive(false);
            }
        }

        if (other.CompareTag("ChangeSceneGate"))
        {
            if (!_inputReader.IsInteract) return;
            other.GetComponent<TriggerChangeScene>().ChangeScene();
            _inputReader.IsInteract = false;
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