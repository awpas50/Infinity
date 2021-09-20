using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarProperties : MonoBehaviour
{
    public float initial_HP;
    public float HP;

    Rigidbody rb;
    GameObject attachedGroundTile;
    public PillarHealthBar pillarHealthBar;
    bool isTouchedGround = false;

    public GameObject VFX;
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
            GameObject a = Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(a, 2f);

            int seed = Random.Range(0, 3);
            switch (seed)
            {
                case 0:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed1);
                    break;
                case 1:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed2);
                    break;
                case 2:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed3);
                    break;
            }

            Destroy(gameObject, 0.02f);
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
