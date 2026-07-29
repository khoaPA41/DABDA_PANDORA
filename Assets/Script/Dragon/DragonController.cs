using UnityEngine;

public class DragonController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    
    private void Start()
    {
        _inputReader = GetComponent<InputReader>();
    }
    
    private void Update()
    {
        
    }
}
