using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public int xRange = 10;            // (+/-) x-range of spawning animals
    public int spawnPosZ = 35;         // How far away (z-axis) do we spawn animals
    public int startDelay = 2;         // How much time after start should we start spawning
    public int height = 100;           // How far above the ground do we want clouds
    public float spawnInterval = 1.5f; // Time interval for spawning clouds
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomCloud", startDelay, spawnInterval); // Repeatedly call spawning method
    }

    void SpawnRandomCloud()
    {
        int cloudIndex = Random.Range(0, cloudPrefabs.Length);                                              // Choosing random cloud
        Vector3 spawnLoc = new Vector3(Random.Range(-xRange, xRange), height, spawnPosZ);                   // Random spawn location

        Instantiate(cloudPrefabs[cloudIndex], spawnLoc, cloudPrefabs[cloudIndex].transform.rotation);       // Spawning cloud
    }
}