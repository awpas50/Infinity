using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private GameObject playerTarget;
    public float distanceToKeep = 8f;
    private float distanceToKeep_real;
    Rigidbody rb;

    public float speed = 3f;

    // 1: follow player, 2: stop
    public int mode = 1;
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        distanceToKeep_real = distanceToKeep - Random.Range(-2, 2f);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Vector3 dir = playerTarget.transform.position - transform.position;
        //Quaternion turnRotation = Quaternion.Euler(dir.x, dir.y, 0);
        //rb.MoveRotation(rb.rotation * turnRotation);
        //if (mode == 1)
        //{
        //    rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        //}
        //if (mode == 2)
        //{
        //    rb.MovePosition(rb.position + dir * speed * Time.deltaTime * 0.1f);
        //}
        //if (mode == 3)
        //{
        //    rb.MovePosition(rb.position + dir * 0 * Time.deltaTime);
        //}

        if (mode == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime);
        }
        if (mode == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.5f);
        }
        if (mode == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0f);
        }

        transform.LookAt(playerTarget.transform);
    }
    void Update()
    {
        // Find player
        if(Vector3.Distance(transform.position, playerTarget.transform.position) <= 4f)
        {
            mode = 3;
        }
        else if(Vector3.Distance(transform.position, playerTarget.transform.position) < distanceToKeep_real)
        {
            mode = 2;
        }
        else if(Vector3.Distance(transform.position, playerTarget.transform.position) >= distanceToKeep_real)
        {
            mode = 1;
        }
    }
}
