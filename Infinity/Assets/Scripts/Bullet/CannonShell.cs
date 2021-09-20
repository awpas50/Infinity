using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShell : MonoBehaviour
{
    private float damage;
    private float speed;
    [SerializeField] private GameObject cannonVFX;
    private GameObject cannonHitVFX;
    Rigidbody rb;
    // Called in PlayerShoot()
    public void SetProperties(float speed, float damage, GameObject cannonHitVFX)
    {
        this.speed = speed;
        this.damage = damage;
        this.cannonHitVFX = cannonHitVFX;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Destroy(gameObject, 10f);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyStat enemyStat = other.gameObject.GetComponent<EnemyStat>();
            enemyStat.TakeDamage(damage);

            Destroy(cannonVFX, 0.2f);
            cannonVFX.transform.parent = null;
            
            StartCoroutine(PlaySoundWithDelay(0.1f));
            Destroy(gameObject);

            GameObject VFXhit = Instantiate(cannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
        if (other.gameObject.tag == "Boss")
        {
            EnemyStat enemyStat = other.gameObject.GetComponent<EnemyStat>();
            enemyStat.TakeDamage(damage);

            Destroy(gameObject);

            GameObject VFXhit = Instantiate(cannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
        if (other.gameObject.tag == "Pillar")
        {
            PillarProperties pillarProperties = other.gameObject.GetComponent<PillarProperties>();
            pillarProperties.TakeDamage(damage);
            pillarProperties.pillarHealthBar.SetAlpha();

            Destroy(cannonVFX, 0.2f);
            cannonVFX.transform.parent = null;
            
            StartCoroutine(PlaySoundWithDelay(0.1f));
            Destroy(gameObject);

            GameObject VFXhit = Instantiate(cannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
        if (other.gameObject.tag == "Player")
        {
            PlayerStat playerStat = other.gameObject.GetComponent<PlayerStat>();
            playerStat.TakeDamage(damage);

            Destroy(gameObject);

            GameObject VFXhit = Instantiate(cannonHitVFX, transform.position, Quaternion.identity);
            Destroy(VFXhit, 2f);
        }
    }

    IEnumerator PlaySoundWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.instance.Play(SoundList.EnemyBeingHit);
    }
}
