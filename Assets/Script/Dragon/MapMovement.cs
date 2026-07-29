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
        // var distance = (transform.position - targetPosition).magnitude;
        // if (distance < 0.1f)
        // {
        //     Debug.Log("Set Active");
        //     gameObject.SetActive(false);
        // }
        if (transform.position.z <= targetPosition.z)
        {
            // gameObject.SetActive(false);
            transform.position = basePosition;
        }
    }
}
