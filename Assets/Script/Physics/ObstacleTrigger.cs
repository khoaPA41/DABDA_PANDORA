using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class TriggerButton
{
    public GameObject button;
    public TextMeshPro text;
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

        StartCoroutine(ChangeOpacity(1f, .3f, 1f,  _nextButton.text));

        if (buttons.IndexOf(_nextButton) == buttons.Count - 1)
        {
            _animator.SetTrigger(ActiveTrigger);
            // gameObject.SetActive(false);
            return;
        }
        
        _nextButton = buttons[buttons.IndexOf(_nextButton) + 1];
        Debug.Log(name + " has been found");
    }

    private IEnumerator ChangeOpacity(float time, float currentOpacity, float targetOpacity, TextMeshPro text)
    {
        var currentColor = text.color;
        var timeElapsed = 0f;

        while (timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;

            var timePercentage = Mathf.Clamp01(timeElapsed / time);
            
            var changeColor = Mathf.Lerp(currentOpacity, targetOpacity, timePercentage);
            
            currentColor.a =  changeColor;
            text.color = currentColor;
            yield return null;
        }
        currentColor.a =  targetOpacity;
        text.color = currentColor;
    }
}