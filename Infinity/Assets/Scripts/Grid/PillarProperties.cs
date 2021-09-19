using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarProperties : MonoBehaviour
{
    private float initial_HP;
    public float HP;

    Rigidbody rb;
    GameObject attachedGroundTile;
    bool isTouchedGround = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initial_HP = HP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject, 0.15f);
        }
    }

    void Update()
    {
        if (!attachedGroundTile && isTouchedGround)
        {
            Rigidbody new_rb = gameObject.AddComponent<Rigidbody>();
            new_rb.isKinematic = false;
            new_rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Ground")
        {
            isTouchedGround = true;
            attachedGroundTile = collision.collider.gameObject;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
