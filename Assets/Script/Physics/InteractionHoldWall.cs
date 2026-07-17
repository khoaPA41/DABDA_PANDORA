using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionHoldWall : MonoBehaviour
{
    [SerializeField] private List<TextMeshPro> gateTextList;
    
    private ForceReceiver _forceReceiver;
    public event Action ClimbAction;
    
    public event Action<string> ActiveButtonTriggerAction;
    public event Action<GameObject> PickUpItemAction;
    public event Action EnterKeyAction;

    private InputReader  _inputReader;
    private void Start()
    {
        _forceReceiver = GetComponent<ForceReceiver>();
        _inputReader = GetComponent<InputReader>();
    }


    public void ActiveText(string name)
    {
        foreach (var text in gateTextList)
        {
            if (text.name == name)
            {
                text.gameObject.SetActive(true);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            transform.SetParent(other.transform);
            _forceReceiver.IsHoldWall = true;
            ClimbAction?.Invoke();
        }
        
        if (other.CompareTag("SlideWall"))
        {
            // transform.SetParent(other.transform);
            _forceReceiver.IsSlideWall = true;
            ClimbAction?.Invoke();
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
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            transform.SetParent(null);
        }
        
        if (other.CompareTag("SlideWall"))
        {
            // transform.SetParent(other.transform);
            _forceReceiver.IsSlideWall = false;
            ClimbAction?.Invoke();
        }
    }
}
