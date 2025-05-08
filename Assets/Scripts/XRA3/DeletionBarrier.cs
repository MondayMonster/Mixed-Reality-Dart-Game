using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BeatSpawnedShape") || other.CompareTag("BeatSpawnedBlock"))
        {
            Destroy(other);
        }
    }
}
