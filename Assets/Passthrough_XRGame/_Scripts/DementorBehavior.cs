using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DementorBehavior : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float moveSpeedOffset = 0.05f;

    private NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);

        _agent = GetComponent<NavMeshAgent>();

        moveSpeed = Random.Range(moveSpeed - moveSpeedOffset, moveSpeed + moveSpeedOffset);
    }

    // Update is called once per frame
    void Update()
    {
        var target = Camera.main.transform.position;

        _agent.SetDestination(target);
        _agent.speed = moveSpeed;
        
        
        //MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        float step = moveSpeed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, Camera.main.transform.position, step);
    }
}
