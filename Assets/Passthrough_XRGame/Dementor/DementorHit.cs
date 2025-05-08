using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DementorHit : MonoBehaviour
{
    public GameObject dementorExpelled;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            DestroyDementor();
        }
    }

    void DestroyDementor()
    {
        Instantiate(dementorExpelled, transform.position, transform.rotation);

        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }
}
