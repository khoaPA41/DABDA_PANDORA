using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileUI : MonoBehaviour
{
    [Header("Text Mesh Animator")] [SerializeField]
    private Animator mainTextAnimator;
    [SerializeField] private Animator subTextAnimator;
    
    [Header("Button UI Canvas Group")] [SerializeField]
    private CanvasGroup buttonCanvasGroup;
    [SerializeField] private float timeToAppear;
    
    private bool isActiveMainAnimation = false;
    private bool isActiveSubAnimation = false;

    private void Start()
    {
        /*Set button canvas group opacity is zero*/
        buttonCanvasGroup.alpha = 0f;
        
        /*UI Appear*/
        StartCoroutine(WaitToActiveAnimation());
    }

    private IEnumerator WaitToActiveAnimation()
    {
        var normalizeTime = 0f;

        yield return null;
        while (normalizeTime < 1f)
        {
            if (!isActiveMainAnimation)
            {
                mainTextAnimator.SetTrigger("Active");
                isActiveMainAnimation = true;
            }

            if (normalizeTime > .9 && !isActiveSubAnimation)
            {
                subTextAnimator.SetTrigger("Active");
                isActiveSubAnimation = true;
            }
            normalizeTime = mainTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            yield return null;
        }
        yield return new  WaitForSeconds(1f);
        StartCoroutine(ButtonAppear());
    }

    private IEnumerator ButtonAppear()
    {
        var startOpacity = buttonCanvasGroup.alpha;
        var elapsedTime = 0f;
        while (elapsedTime < timeToAppear)
        {
            elapsedTime  += Time.deltaTime;
            var timePercentage = Mathf.Clamp01(elapsedTime / timeToAppear);
            buttonCanvasGroup.alpha  = Mathf.Lerp(startOpacity, 1, timePercentage);
            yield return null;
        }
        buttonCanvasGroup.alpha = 1f;
    }
}