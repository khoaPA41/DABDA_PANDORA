using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveGate : MonoBehaviour
{
    [SerializeField] private List<TextMeshPro> texts;

    private InputReader _inputReader;

    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        // if(other.tag == "Player")
    }
}
