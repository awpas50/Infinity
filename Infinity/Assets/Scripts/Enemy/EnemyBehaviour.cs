using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject playerTarget;
    public float distanceToKeep = 8f;
    private float distanceToKeep_real;
    Rigidbody rb;

    public float speed = 3f;
    public GameObject cannonShell;
    public float shellSpeed = 35f;
    public float shellDamage = 30;
    public float cannonCD = 5;
    private float cannonTimer;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject firePoint;

    private float restTimer;
    // 1: follow player, 2: stop
    public int mode = 1;

    [Header("Particle effect")]
    [SerializeField] private GameObject cannon_VFX1;
    [SerializeField] private GameObject cannon_hit;

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        distanceToKeep_real = distanceToKeep - Random.Range(-2, 2f);
        rb = GetComponent<Rigidbody>();

        restTimer = Random.Range(1, 4);
        cannonTimer = 1f;
        StartCoroutine(BackgroundTimer());
    }

    private void FixedUpdate()
    {
        if(playerTarget)
        {
            if (restTimer % 5 == 0)
            {
                Vector3 relativePos = playerTarget.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);
                if (mode == 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.05f);
                }
                if (mode == 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.05f);
                }
                if (mode == 3)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0f);
                }
            }
            if (restTimer % 5 == 1 || restTimer % 5 == 2 || restTimer % 5 == 3 || restTimer % 5 == 4)
            {
                Vector3 relativePos = playerTarget.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);
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
            }
        }
    }
    void Update()
    {
        // Find player
        if(playerTarget)
        {
            if (Vector3.Distance(transform.position, playerTarget.transform.position) <= 4f)
            {
                mode = 3;
            }
            else if (Vector3.Distance(transform.position, playerTarget.transform.position) < distanceToKeep_real)
            {
                mode = 2;
            }
            else if (Vector3.Distance(transform.position, playerTarget.transform.position) >= distanceToKeep_real)
            {
                mode = 1;
            }
            barrel.transform.LookAt(playerTarget.transform);
        }
        
        // Shoot
        cannonTimer += Time.deltaTime;
        if(cannonTimer >= Random.Range(cannonCD - 0.4f, cannonCD + 0.4f))
        {
            AudioManager.instance.Play(SoundList.EnemyCannonSound1);
            GameObject newShell = Instantiate(cannonShell, firePoint.transform.position, firePoint.transform.rotation);
            CannonShell cannonShellScript = newShell.GetComponent<CannonShell>();
            cannonShellScript.SetProperties(shellSpeed, shellDamage, cannon_hit);

            GameObject newVFX = Instantiate(cannon_VFX1, firePoint.transform.position, firePoint.transform.rotation);
            newVFX.transform.SetParent(firePoint.transform);
            Destroy(newVFX, 4f);

            cannonTimer = 0f;
        }
    }

    IEnumerator BackgroundTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            restTimer++;
        }
    }
}
