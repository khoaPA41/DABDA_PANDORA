using UnityEngine;

public class ObstacleRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis;    
    [SerializeField] private float rotateSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(rotationAxis, rotateSpeed * Time.deltaTime);
    }
}
