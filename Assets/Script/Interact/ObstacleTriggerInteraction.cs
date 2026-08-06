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

public class ObstacleTriggerInteraction : MonoBehaviour
{
    [Header("Animation Trigger Name")]
    private static readonly int ActiveTrigger = Animator.StringToHash("ActiveTrigger");
    
    [Header("Object")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<TriggerButton> buttons;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip tremorSound;

    private TriggerButton _nextButton;
    private Interaction _player;
    private Rigidbody _rb;
    private Animator _animator;
    private Color _currentColor;

    private MapManagers _mapManagers;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();
        _mapManagers =  GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManagers>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        _nextButton = buttons[0];
        _player.ActiveButtonTriggerAction += FindButton;
        _currentColor = buttons[0].text.color;
    }
    
    private void OnDisable()
    {
        _player.ActiveButtonTriggerAction -= FindButton;
    }

    private void ResetButtons()
    {
        _nextButton = buttons[0];
        foreach (var button in buttons)
        {
            button.text.color = _currentColor;
        }
    }

    private TextMeshPro FindButtonByName(string name)
    {
        foreach (var button in buttons)
        {
            if (button.button.name == name)
            {
                return button.text;
            }
        }
        return null;
    }

    private void FindButton(string name)
    {
        if (_nextButton.button.name != name)
        {
            PlayButtonSound(wrongSound);
            StartCoroutine(ChangeAlertColor(1f, FindButtonByName(name)));
            return;
        }

        PlayButtonSound(correctSound);

        StartCoroutine(ChangeOpacity(1f, .3f, 1f,  _nextButton.text));

        if (buttons.IndexOf(_nextButton) == buttons.Count - 1)
        {
            PlayButtonSound(tremorSound);
            _animator.SetTrigger(ActiveTrigger);
            GameManager.Instance.obstacleTrigger_I = true;
            return;
        }
        _nextButton = buttons[buttons.IndexOf(_nextButton) + 1];
    }

    private IEnumerator ChangeAlertColor(float time, TextMeshPro text)
    {
        text.color = Color.red;
        yield return new WaitForSeconds(time);
        text.color = _currentColor;
        ResetButtons();
    }

    private static IEnumerator ChangeOpacity(float time, float currentOpacity, float targetOpacity, TextMeshPro text)
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
    
    private void PlayButtonSound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}