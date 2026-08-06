using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    public ObjectPooling ObjectPooling;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ObjectPooling = GetComponent<ObjectPooling>();
    }
}
