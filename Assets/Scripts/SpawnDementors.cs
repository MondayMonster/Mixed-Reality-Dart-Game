using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDementors : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnFrequency = 2f;
    public int spawnLimit = 5;

    private bool isSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnDementor", 0, spawnFrequency);
    }

    private void Update()
    {
        bool isSpawning = GameObject.FindGameObjectsWithTag("Dementor").Length <= spawnLimit;
    }


    public void SpawnDementor()
    {
        if (isSpawning)
        {
            Instantiate(enemyPrefab, this.transform.position, this.transform.rotation);
        }
    }
}
