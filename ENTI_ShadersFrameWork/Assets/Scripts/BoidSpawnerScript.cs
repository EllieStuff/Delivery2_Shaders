using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawnerScript : MonoBehaviour
{
    public BoidAgentScript boidPrefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;

    void Awake()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Instantiate(boidPrefab, pos, Random.rotation, transform);
        }
    }

}