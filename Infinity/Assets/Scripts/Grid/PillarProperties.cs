using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarProperties : MonoBehaviour
{
    Rigidbody rb;
    GameObject attachedGroundTile;
    bool isTouchedGround = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!attachedGroundTile && isTouchedGround)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Ground")
        {
            isTouchedGround = true;
            attachedGroundTile = collision.collider.gameObject;
            Destroy(rb);
        }
    }
}
