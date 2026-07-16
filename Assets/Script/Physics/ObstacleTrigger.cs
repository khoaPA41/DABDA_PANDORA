using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


[Serializable]
public class TriggerButton
{
    public GameObject button;
}

public class ObstacleTrigger : MonoBehaviour
{
    private static readonly int ActiveTrigger = Animator.StringToHash("ActiveTrigger");
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<TriggerButton> buttons;

    private TriggerButton _nextButton;
    private InteractionHoldWall _player;
    private Rigidbody _rb;
    private Animator _animator;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionHoldWall>();
        _animator = GetComponent<Animator>();
        _nextButton = buttons[0];
        _player.ActiveButtonTriggerAction += FindButton;
    }
    
    private void OnDisable()
    {
        _player.ActiveButtonTriggerAction -= FindButton;
    }

    private void FindButton(string name)
    {
        if (_nextButton.button.name != name) return;

        if (buttons.IndexOf(_nextButton) == buttons.Count - 1)
        {
            _animator.SetTrigger(ActiveTrigger);
            // gameObject.SetActive(false);
            return;
        }

        _nextButton = buttons[buttons.IndexOf(_nextButton) + 1];
        Debug.Log(name + " has been found");
    }
}