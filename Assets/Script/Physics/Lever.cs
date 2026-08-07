using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private float forcePush;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var forceReceiver = other.GetComponent<ForceReceiver>();
        forceReceiver.Jump(forcePush);
    }
}
