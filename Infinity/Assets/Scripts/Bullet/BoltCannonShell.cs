using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltCannonShell : MonoBehaviour
{
    private float damage;
    private float speed;
    [SerializeField] private GameObject boltCannonVFX;
    [SerializeField] private ParticleSystem VFX1;
    [SerializeField] private ParticleSystem VFX2;
    [SerializeField] private ParticleSystem VFX3;

    [SerializeField] private GameObject boltCannonHitVFX;

    Rigidbody rb;

    // Called in PlayerShoot()
    public void SetProperties(float speed, float damage, GameObject boltCannonHitVFX)
    {
        this.speed = speed;
        this.damage = damage;
        this.boltCannonHitVFX = boltCannonHitVFX;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Destroy(gameObject, 5f);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Enemy")
        //{
        //    EnemyStat enemyStat = other.gameObject.GetComponent<EnemyStat>();
        //    enemyStat.TakeDamage(damage);
        //    StartCoroutine(PlaySoundWithDelay(0.1f));
        //    Destroy(gameObject, 0.05f);
        //}

        if (other.gameObject.tag == "Enemy")
        {
            EnemyStat enemyStat = other.gameObject.GetComponent<EnemyStat>();
            enemyStat.TakeDamage(damage);

            GameObject VFXhit = Instantiate(boltCannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
        if (other.gameObject.tag == "Pillar")
        {
            PillarProperties pillarProperties = other.gameObject.GetComponent<PillarProperties>();
            pillarProperties.TakeDamage(damage);
            //Destroy(gameObject);

            GameObject VFXhit = Instantiate(boltCannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
    }

    IEnumerator PlaySoundWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.instance.Play(SoundList.EnemyBeingHit);
    }
}
