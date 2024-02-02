using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour
{
    public GameObject Shark1;
    public GameObject Shark2;
    public GameObject Shark3;
    public List<Transform> spawnPoints; // Ensure this list has at least 3 elements

    public float minSpawnDelay = 2.0f; // Minimum time between spawns
    public float maxSpawnDelay = 10.0f; // Maximum time between spawns

    void Start()
    {
        // Start spawning routines for each shark
        StartCoroutine(SpawnSharkAtInterval(Shark1, spawnPoints[0]));
        StartCoroutine(SpawnSharkAtInterval(Shark2, spawnPoints[1]));
        StartCoroutine(SpawnSharkAtInterval(Shark3, spawnPoints[2]));
    }

    IEnumerator SpawnSharkAtInterval(GameObject sharkPrefab, Transform spawnPoint)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            Instantiate(sharkPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
