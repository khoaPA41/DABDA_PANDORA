using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Script.Design_Pattern.Object_Pooling;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Enemy Information Settings")]
    [SerializeField] private DragonEnemyData[] dragonData;
    [SerializeField] private float minInterval = 1.5f;
    [SerializeField] private float maxInterval = 3f;
    [SerializeField] private float timeToSpawnBoss;
    
    [Header("Boss")]
    [SerializeField] private GameObject dragonBoss;

    private Camera mainCamera;
    private float spawnDepth;
    private float countRealTime;
    
    private void Start()
    {
        mainCamera  = Camera.main;
        countRealTime = Time.time + timeToSpawnBoss;
    }

    private void OnEnable()
    {
        if (ObjectPooling.Instance == null) return;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (Time.time < countRealTime)
        {
            var randomTimeToSpawn = Random.Range(minInterval, maxInterval);
            yield return new WaitForSecondsRealtime(randomTimeToSpawn);
            Spawn();
        }
        dragonBoss.SetActive(true);
    }

    private void Spawn()
    {
        if (mainCamera is null) return;
        var data = dragonData[Random.Range(0, dragonData.Length)];

        var viewPos = new Vector3(Random.Range(.1f, .9f), 2f, 59.8f);
        var worldSpacePos = mainCamera.ViewportToWorldPoint(viewPos);
        
        // var enemy = ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(data.name, worldSpacePos);
        var enemy = ObjectPooling.Instance.GetPooledObject(data.name, worldSpacePos);
        enemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        enemy.GetComponent<DragonEnemy>().Init(data);
    }
}
