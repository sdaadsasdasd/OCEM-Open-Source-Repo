using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public int xRange = 10;            // (+/-) x-range of spawning clouds
    public int spawnPosZ = 35;         // How far away (z-axis) do we spawn clouds
    public int startDelay = 2;         // How much time after start should we start spawning
    public int height = 100;           // How far above the ground do we want clouds
    public int yRange = 10;            // (+/-) y-range of spawning clouds
    public int scaling = 5;            // [1, scaling] scaling range of clouds
    public float spawnInterval = 1.5f; // Time interval for spawning clouds
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomCloud", startDelay, spawnInterval); // Repeatedly call spawning method
    }

    void SpawnRandomCloud()
    {
        int cloudIndex = Random.Range(0, cloudPrefabs.Length);                                              // Choosing random cloud
        Vector3 spawnLoc = new Vector3(Random.Range(-xRange, xRange), height + Random.Range(-yRange, yRange), spawnPosZ);                   // Random spawn location

        GameObject tempObject = Instantiate(cloudPrefabs[cloudIndex], spawnLoc, cloudPrefabs[cloudIndex].transform.rotation);       // Spawning cloud
        float tempScale = Random.Range(1, scaling);
        tempObject.transform.localScale = new Vector3(tempScale, tempScale, tempScale);               // change its local scale in x y z format
    }
}