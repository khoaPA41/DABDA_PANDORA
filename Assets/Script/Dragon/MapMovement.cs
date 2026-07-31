using UnityEngine;

public class MapMovement : MonoBehaviour
{
    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float speed;
    
    private void Update()
    {
        Move();
        CheckPosition();
    }

    private void OnEnable()
    {
        transform.position = basePosition;
    }

    private void Move()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    private void CheckPosition()
    {
        if (transform.position.z <= targetPosition.z)
        {
            transform.position = basePosition;
        }
    }
}
