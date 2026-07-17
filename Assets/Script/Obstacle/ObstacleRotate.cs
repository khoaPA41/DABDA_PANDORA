using System.Collections;
using UnityEngine;

public class ObstacleRotate : MonoBehaviour
{
    [SerializeField] private Quaternion rotationAxis;    
    [SerializeField] private float timeWaitToNextRotate;
    [SerializeField] private float timeToDoneRotation;
    
    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            var startQuaternion = transform.rotation;
            var targetQuaternion = startQuaternion * rotationAxis;

            var timeElapsed = 0f;

            while (timeElapsed < timeToDoneRotation)
            {
                var timePercentage = Mathf.Clamp01(timeElapsed / timeToDoneRotation);
                
                transform.rotation = Quaternion.Lerp(startQuaternion, targetQuaternion, timePercentage);
                
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetQuaternion;
            yield return new WaitForSeconds(timeWaitToNextRotate);
        }
    }
}
