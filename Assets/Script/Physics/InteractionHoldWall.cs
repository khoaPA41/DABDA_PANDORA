using System;
using UnityEngine;

public class InteractionHoldWall : MonoBehaviour
{
    private ForceReceiver _forceReceiver;
    public event Action ClimbAction;
    private void Start()
    {
        _forceReceiver = GetComponent<ForceReceiver>();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanHold"))
        {
            _forceReceiver.IsHoldWall = false;
            transform.SetParent(null);
        }
    }
}
