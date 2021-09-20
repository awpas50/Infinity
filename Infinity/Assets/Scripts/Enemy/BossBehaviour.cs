using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    EnemyStat enemyStat;
    public GameObject playerTarget;
    public float distanceToKeep = 8f;
    private float distanceToKeep_real;
    Rigidbody rb;

    public float speed = 3f;
    public GameObject cannonShell;
    public float shellSpeed = 35f;
    public float shellDamage = 30;
    public float cannonCD = 8;
    private float cannonTimer;
    [SerializeField] private GameObject firePoint1;
    [SerializeField] private GameObject firePoint2;
    [SerializeField] private GameObject firePoint3;
    [SerializeField] private GameObject firePoint4;
    [SerializeField] private GameObject firePoint5;
    private float restTimer;
    // 1: follow player, 2: stop
    public int mode = 1;

    [Header("Particle effect")]
    [SerializeField] private GameObject cannon_VFX1;
    [SerializeField] private GameObject cannon_hit;

    void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        distanceToKeep_real = distanceToKeep;
        rb = GetComponent<Rigidbody>();

        restTimer = 1;
        cannonTimer = 1f;
        StartCoroutine(BackgroundTimer());
    }

    private void FixedUpdate()
    {
        if (playerTarget)
        {
            if (restTimer % 6 == 5 || restTimer % 6 == 0)
            {
                Vector3 relativePos = playerTarget.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);
                if (mode == 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 1f);
                }
                if (mode == 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.8f);
                }
                if (mode == 3)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.1f);
                }
            }
            if (restTimer % 6 == 1 || restTimer % 6 == 2 || restTimer % 6 == 3 || restTimer % 6 == 4)
            {
                Vector3 relativePos = playerTarget.transform.position - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1.5f * Time.deltaTime);
                if (mode == 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.12f);
                }
                if (mode == 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, speed * Time.deltaTime * 0.08f);
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
        if (playerTarget)
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
            transform.LookAt(playerTarget.transform);
        }

        if (enemyStat.HP / enemyStat.initial_HP <= 0.5f)
        {
            cannonCD = 4.5f;
        }
        else
        {
            cannonCD = 8f;
        }
        // Shoot
        cannonTimer += Time.deltaTime;
        if (cannonTimer >= cannonCD)
        {
            AudioManager.instance.Play(SoundList.BossShoot);

            GameObject newShell1 = Instantiate(cannonShell, firePoint1.transform.position, firePoint1.transform.rotation);
            GameObject newShell2 = Instantiate(cannonShell, firePoint2.transform.position, firePoint2.transform.rotation);
            GameObject newShell3 = Instantiate(cannonShell, firePoint3.transform.position, firePoint3.transform.rotation);

            CannonShell cannonShellScript1 = newShell1.GetComponent<CannonShell>();
            CannonShell cannonShellScript2 = newShell2.GetComponent<CannonShell>();
            CannonShell cannonShellScript3 = newShell3.GetComponent<CannonShell>();

            cannonShellScript1.SetProperties(shellSpeed, shellDamage, cannon_hit);
            cannonShellScript2.SetProperties(shellSpeed, shellDamage, cannon_hit);
            cannonShellScript3.SetProperties(shellSpeed, shellDamage, cannon_hit);

            if (enemyStat.HP / enemyStat.initial_HP <= 0.5f)
            {
                GameObject newShell4 = Instantiate(cannonShell, firePoint4.transform.position, firePoint4.transform.rotation);
                GameObject newShell5 = Instantiate(cannonShell, firePoint5.transform.position, firePoint5.transform.rotation);

                CannonShell cannonShellScript4 = newShell4.GetComponent<CannonShell>();
                CannonShell cannonShellScript5 = newShell5.GetComponent<CannonShell>();

                cannonShellScript4.SetProperties(shellSpeed, shellDamage, cannon_hit);
                cannonShellScript5.SetProperties(shellSpeed, shellDamage, cannon_hit);
            }

            GameObject newVFX = Instantiate(cannon_VFX1, firePoint1.transform.position, firePoint1.transform.rotation);
            newVFX.transform.SetParent(firePoint1.transform);
            Destroy(newVFX, 4f);

            cannonTimer = 0f;
        }
    }

    IEnumerator BackgroundTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            restTimer++;
        }
    }
}
