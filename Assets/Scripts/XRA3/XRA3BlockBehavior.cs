using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRA3BlockBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (XRA3LevelManager.Instance.gameOver) Destroy(gameObject);
        this.transform.position += moveSpeed * Time.deltaTime * transform.forward * -1;
    }
}
