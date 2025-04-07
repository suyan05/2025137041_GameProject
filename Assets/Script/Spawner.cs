using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject CoinPrefabs;
    public GameObject MissilPrefabs;

    [Header("스폰확률")]
    [Range(0, 100)]
    public int coinSpawnChance = 50;

    [Header("스폰 타이밍")]
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;

    public float Timer = 0.0f;
    public float nextSpawnTime;

    [Header("스폰움직임")]
    public float initpositionX;

    public float distance;
    public float turningPoint;
    
    public bool turnSwitch;
    public float SpawnerMoveSpeed;

    private void Awake()
    {
        if(gameObject.name =="Spawner")
        {
            initpositionX = transform.position.x;
            turningPoint = initpositionX - distance;
        }
    }

    private void Start()
    {
        SetNextpawn();
    }

    private void Update()
    {
        SpawnTime();
        SpawnerMove();
    }

    private void SetNextpawn()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnTime()
    {
        Timer += Time.deltaTime;
        if(Timer>=nextSpawnTime)
        {
            Timer = 0.0f;
            SpawnObj();
            SetNextpawn();
        }
    }

    private void SpawnObj()
    {
        Transform spawnTransform = transform;

        int randomValue = Random.Range(0, 100);
        if(randomValue<coinSpawnChance)
        {
            Instantiate(CoinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
    }

    private void SpawnerMove()
    {
        float currentPositionX = transform.position.x;

        if (currentPositionX >= initpositionX + distance)
        {
            turnSwitch = false;
        }
        else if (currentPositionX <= turningPoint)
        {
            turnSwitch = true;
        }

        if (turnSwitch)
        {
            transform.position = transform.position + new Vector3(1, 0, 0) * SpawnerMoveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + new Vector3(-1, 0, 0) * SpawnerMoveSpeed * Time.deltaTime;
        }
    }
}
