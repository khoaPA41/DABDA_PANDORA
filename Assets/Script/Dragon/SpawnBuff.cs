using System;
using System.Collections;
using System.Collections.Generic;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnBuff : MonoBehaviour
{
    [SerializeField] private List<string> buffNames;
    [SerializeField] private float timeToNextSpawn;

    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToNextSpawn);
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_mainCamera is null) return;
        var randomName = buffNames[Random.Range(0, buffNames.Count)];
        var viewPoint = new Vector3(Random.Range(.1f, .9f), 1.1f, 58f);
        var worldSpacePos = _mainCamera.ViewportToWorldPoint(viewPoint);
        ObjectPooling.Instance.GetPooledObject(randomName, worldSpacePos);
    }
 
}
