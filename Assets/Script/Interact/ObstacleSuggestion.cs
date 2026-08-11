using System;
using System.Collections;
using UnityEngine;

public class ObstacleSuggestion : MonoBehaviour
{
    [Header("Camera")] [SerializeField] private GameObject suggestionCamera;
    [Header("Time To Back Normal Camera")] [SerializeField] private float time;


    public void Suggestion()
    {
        StartCoroutine(ActiveSuggestCamera());
    }
    
    private IEnumerator ActiveSuggestCamera()
    {
        suggestionCamera.SetActive(true);
        yield return new WaitForSecondsRealtime(time);
        suggestionCamera.SetActive(false);
    }
}
