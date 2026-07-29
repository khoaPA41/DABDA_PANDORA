using UnityEngine;
using UnityEngine.AI;

public class ZombieSpline : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _player;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }
}
