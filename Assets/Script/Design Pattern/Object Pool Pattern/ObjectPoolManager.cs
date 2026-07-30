using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    [SerializeField] private ObjectPooling objectPool;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ObjectPooling ObjectPooling => objectPool;
}
