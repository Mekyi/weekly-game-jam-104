using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] boxPrefabs;
    [SerializeField] float spawnFrequency = 1f;
    [SerializeField] float spawnTimer;


    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        GetSpawnFrequency();

        if (spawnTimer <= 0f)
        {
            Instantiate(boxPrefabs[UnityEngine.Random.Range(0, boxPrefabs.Length)]);
            spawnTimer = spawnFrequency;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void GetSpawnFrequency()
    {
        spawnFrequency = FindObjectOfType<GameSession>().boxFrequency;
    }
}
