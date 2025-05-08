using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRA3ShapeSpawner : MonoBehaviour
{
    public GameObject[] spawnableObjects;
    public GameObject block;
    public float x_Offset = 0.8f;
    public float y_aboveOffset = 0.5f;
    public float y_belowOffset = 0.1f;
    public float offsetBlock = 1f;

    public float minWaitTime = 2f;
    public float maxWaitTime = 8f;

    public bool stopSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnRandomBlock());
    }

    private void Update()
    {
     
    }

    private IEnumerator SpawnRandomBlock() {

        while (!stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            if (stopSpawning) break;

            float newX = Random.Range(this.transform.position.x - x_Offset, this.transform.position.x + x_Offset);
            float newY = Random.Range(this.transform.position.y - y_belowOffset, this.transform.position.y + y_aboveOffset);

            GameObject spawnedObject = Instantiate(block, new Vector3(newX, newY - offsetBlock, this.transform.position.z), Quaternion.identity);
            Vector3 newScale = new(Random.Range(1f, 3f), Random.Range(0.8f, 2f), spawnedObject.transform.localScale.z);
            spawnedObject.transform.localScale = newScale;
        }
       
    }

    public void SpawnObjectToBeat()
    {
        if (!stopSpawning)
        {
            GameObject objectToSpawn = spawnableObjects[Random.Range(0, spawnableObjects.Length)];
            float newX = Random.Range(this.transform.position.x - x_Offset, this.transform.position.x + x_Offset);
            float newY = Random.Range(this.transform.position.y - y_belowOffset, this.transform.position.y + y_aboveOffset);
            GameObject spawnedObject = Instantiate(objectToSpawn, new Vector3(newX, newY, this.transform.position.z), Quaternion.identity);
        }
    }
}
