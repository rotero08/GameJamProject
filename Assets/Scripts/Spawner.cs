using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject piratePrefab;
    public int maxPirates = 10;

    private int piratesDestroyed = 0;
    private bool isSpawning = true;

    private void Start()
    {
        SpawnPirate();
    }

    private void SpawnPirate()
    {
        if (piratesDestroyed >= maxPirates)
        {
            isSpawning = false;
            return;
        }

        Instantiate(piratePrefab, transform.position, Quaternion.identity);

        piratesDestroyed++;

        if (isSpawning)
            Invoke("SpawnPirate", 10f);
    }

    public void PirateDestroyed()
    {
        piratesDestroyed--;
    }
}
