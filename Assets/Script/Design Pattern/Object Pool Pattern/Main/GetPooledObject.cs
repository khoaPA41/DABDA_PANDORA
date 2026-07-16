using Script.Design_Pattern.Object_Pooling;
using UnityEngine;

public class GetPooledObject : MonoBehaviour
{
    private ObjectPooling _objectPooling;

    public PooledObject item {get; private set;}
    private void Start()
    {
        _objectPooling = GameObject.FindWithTag("ObjectPooling").GetComponent<ObjectPooling>();
    }

    public void GetObject(string name, Vector3 position, Transform parent)
    {
        item = _objectPooling.GetPooledObject(name, position);
        item.gameObject.transform.SetParent(parent, false);
        item.GetComponent<BoxCollider>().enabled = false;
    }
}

