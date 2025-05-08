using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartSpawner_XRA4 : MonoBehaviour
{
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private float spawnFrequency = 1f;
    [SerializeField] private int activeDartLimit = 3;
    public static int activeDartCount = 0;
    [SerializeField] private float x_offset = 0.005f;
    [SerializeField] private float y_offset = 0.02f;

    [SerializeField] private AudioSource _audioSource;

    public bool keepSpawning = true;
    

    // Start is called before the first frame update
    void Start()
    {
        activeDartCount = 0;
        InvokeRepeating("SpawnDart", spawnFrequency, spawnFrequency);
        _audioSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool limitReached = activeDartCount >= activeDartLimit;

        keepSpawning = !limitReached;
    }

    private void SpawnDart()
    {
        if (keepSpawning)
        {
            Debug.Log("Spawning new dart!");
            _audioSource.Play();
            Vector3 newDartPosition = new Vector3(transform.position.x, transform.position.y + y_offset, transform.position.z + Random.Range(-x_offset, x_offset));
            GameObject spawnedDart = Instantiate(dartPrefab, newDartPosition, transform.rotation);
            spawnedDart.transform.Rotate(-90f, 0, 0);
            spawnedDart.transform.SetParent(this.transform);
            activeDartCount++;
        }
    }
}
