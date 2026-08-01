using System;
using UnityEngine;

public class TriggerAutoSave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AutoSave();
        }
    }
}
