using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime = 3.0f;

    public float minEdgeDistance = 0.5f;
    public MRUKAnchor.SceneLabels spawnLabels;

    public float xMin = -25;
    public float xMax = 25;
    public float yMin = 8;
    public float yMax = 25;
    public float zMin = -25;
    public float zMax = 25;

    void Start()
    {
        if(MRUK.Instance && MRUK.Instance.IsInitialized)
        {
            InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
        }

        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);

    }

    void SpawnEnemies()
    {

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();


        Vector3 enemyPosition;

        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), 
            out Vector3 enemyPos, out Vector3 norm);

        

        enemyPosition.x = Random.Range(xMin, xMax);
        enemyPosition.y = Random.Range(yMin, yMax);
        enemyPosition.z = Random.Range(zMin, zMax);


        GameObject spawnedEnemy = 
            Instantiate(enemyPrefab, enemyPosition, transform.rotation)
            as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
    }
}
