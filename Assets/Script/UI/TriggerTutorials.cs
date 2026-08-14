using System.Collections;
using UnityEngine;

public class TriggerTutorials : MonoBehaviour
{
    private static readonly int AppearHash = Animator.StringToHash("Appear");
    private static readonly int DisappearHash = Animator.StringToHash("Disappear");

    [Header("Tutorial UI")]
    [SerializeField]
    private Animator tutorialAnimator;

    [Header("Time To UnActive Tutorial")]
    [SerializeField]
    private float time;
    private IEnumerator TimeToInActiveTutorials()
    {
        tutorialAnimator.SetTrigger(AppearHash);
        yield return new WaitForSecondsRealtime(time);
        tutorialAnimator.SetTrigger(DisappearHash);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TimeToInActiveTutorials());
        }
    }
}
