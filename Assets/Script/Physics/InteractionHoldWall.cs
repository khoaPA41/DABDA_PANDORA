using System;
using UnityEngine;

public class InteractionHoldWall : MonoBehaviour
{
    private ForceReceiver _forceReceiver;
    public event Action ClimbAction;
    
    public event Action<string> ActiveButtonTriggerAction;

    private InputReader  _inputReader;
    private void Start()
    {
        _forceReceiver = GetComponent<ForceReceiver>();
        _inputReader = GetComponent<InputReader>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            transform.SetParent(other.transform);
            _forceReceiver.IsHoldWall = true;
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
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            _forceReceiver.IsHoldWall = false;
            transform.SetParent(null);
        }
    }
}
