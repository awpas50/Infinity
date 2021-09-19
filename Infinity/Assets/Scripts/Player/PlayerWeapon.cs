using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Props")]
    private PlayerWeaponSelection playerWeaponSelection;
    [SerializeField] private FirePointRandomAngle firePointRandomAngle;
    [SerializeField] private GameObject firePointCannon;
    [SerializeField] private GameObject firePointMachineGun;
    [Header("Cannon")]
    [SerializeField] private GameObject shell;
    [SerializeField] private float shellSpeed;
    [SerializeField] private float shellDamage = 50f;
    private float cannonTimer = 0f;
    [SerializeField] private float cannonCD = 2f;
    [Header("Machine gun")]
    [SerializeField] private GameObject machineGunBullet;
    private float machineGunTimer = 0f;
    [SerializeField] private float machineGunCD = 0.08f;
    [SerializeField] private int machineGunAmmo = 30;
    private float machineGunReloadTimer = 0f;
    [SerializeField] private float machineGunReloadTime = 4f;
    [SerializeField] private float machineGunDamage = 7f;
    [Header("Bolt Cannon")]
    [SerializeField] private GameObject boltShell;
    [SerializeField] private float boltShellSpeed;
    [SerializeField] private float chargeTime = 2f;
    private float chargeTimer;
    [SerializeField] private float boltCannonCD = 2f;
    private float boltCannonTimer = 0f;
    [SerializeField] private float boltShellDamage = 75f;

    [Header("Particle effect")]
    [SerializeField] private GameObject cannon_VFX1;
    [SerializeField] private GameObject cannon_VFX2;
    [SerializeField] private GameObject cannon_hit;
    [SerializeField] private GameObject machineGun_VFX_origin;
    [SerializeField] private GameObject machineGun_VFX_fly;
    [SerializeField] private GameObject machineGun_VFX_hit;
    [SerializeField] private GameObject boltCannon_VFX1;
    [SerializeField] private GameObject boltCannon_VFX2;
    [SerializeField] private GameObject boltCannon_hit;
    [SerializeField] private GameObject missile_VFX;
    [Header("Laser Sight")]
    [SerializeField] private LineRenderer lr;

    void Start()
    {
        playerWeaponSelection = GetComponent<PlayerWeaponSelection>();

        cannonTimer = cannonCD;
        machineGunTimer = machineGunCD;
        boltCannonTimer = boltCannonCD;

        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }
    
    void Update()
    {
        cannonTimer += Time.deltaTime;
        machineGunTimer += Time.deltaTime;
        boltCannonTimer += Time.deltaTime;

        ActivateLaserSight();
        CheckMachineGunReload();


        // Cannon
        if (playerWeaponSelection.GetCurrentWeaponID() == 1) {
            firePointRandomAngle.ResetFirePointAngle();
            if(Input.GetMouseButtonDown(0) && cannonTimer >= cannonCD)
            {
                AudioManager.instance.Play(SoundList.CannonSound1);
                GameObject newShell = Instantiate(shell, firePointCannon.transform.position, firePointCannon.transform.rotation);
                CannonShell cannonShellScript = newShell.GetComponent<CannonShell>();
                cannonShellScript.SetProperties(shellSpeed, shellDamage, cannon_hit);

                cannonTimer = 0f;

                GameObject newVFX = Instantiate(cannon_VFX1, firePointCannon.transform.position, firePointCannon.transform.rotation);
                newVFX.transform.SetParent(firePointCannon.transform);
                Destroy(newVFX, 4f);

                GameObject newVFX2 = Instantiate(cannon_VFX2, firePointCannon.transform.position, firePointCannon.transform.rotation);
                Destroy(newVFX2, 4f);
            }
            else if(Input.GetMouseButtonDown(0) && cannonTimer < cannonCD)
            {
                AudioManager.instance.Play(SoundList.OutOfAmmo);
            }
        }
        // Machine Gun
        if (playerWeaponSelection.GetCurrentWeaponID() == 2)
        {
            firePointRandomAngle.RandomProjectile();
            if (Input.GetMouseButtonDown(0) && machineGunAmmo <= 0)
            {
                AudioManager.instance.Play(SoundList.OutOfAmmo);
            }
            else if (Input.GetMouseButton(0) && machineGunTimer >= machineGunCD && machineGunAmmo > 0)
            {
                AudioManager.instance.Play(SoundList.MachineGunSound2);
                GameObject newBullet = Instantiate(machineGunBullet, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                MachineGunAmmo machineGunAmmoScript = newBullet.GetComponent<MachineGunAmmo>();

                GameObject newVFX = Instantiate(machineGun_VFX_origin, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                newVFX.transform.SetParent(firePointMachineGun.transform);
                Destroy(newVFX, 3f);

                GameObject newVFX2 = Instantiate(machineGun_VFX_fly, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                Destroy(newVFX2, 3f);

                GameObject newVFX3 = Instantiate(machineGun_VFX_hit, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                Destroy(newVFX3, 3f);

                machineGunAmmoScript.SetProperties(firePointMachineGun, newVFX2, newVFX3, machineGunDamage);
                machineGunAmmoScript.CalculateHitPositionAndDestroyAmmo();

                machineGunTimer = 0f;
                machineGunAmmo--;
            }
        }

        // Bolt cannon
        if(playerWeaponSelection.GetCurrentWeaponID() == 3)
        {
            firePointRandomAngle.ResetFirePointAngle();
            if (Input.GetMouseButtonDown(0) && boltCannonTimer >= boltCannonCD)
            {
                AudioManager.instance.Play(SoundList.BoltCannonSound1);
                GameObject newboltShell = Instantiate(boltShell, firePointCannon.transform.position, firePointCannon.transform.rotation);
                BoltCannonShell boltCannonShellScript = newboltShell.GetComponent<BoltCannonShell>();
                boltCannonShellScript.SetProperties(boltShellSpeed, boltShellDamage, boltCannon_hit);

                boltCannonTimer = 0f;

                GameObject newVFX = Instantiate(boltCannon_VFX1, firePointCannon.transform.position, firePointCannon.transform.rotation);
                newVFX.transform.SetParent(firePointCannon.transform);
                Destroy(newVFX, 4f);

                GameObject newVFX2 = Instantiate(boltCannon_VFX2, firePointCannon.transform.position, firePointCannon.transform.rotation);
                Destroy(newVFX2, 4f);
            }
            else if (Input.GetMouseButtonDown(0) && boltCannonTimer < boltCannonCD)
            {
                AudioManager.instance.Play(SoundList.OutOfAmmo);
            }
        }
        if (playerWeaponSelection.GetCurrentWeaponID() == 4)
        {

        }
    }

    void ActivateLaserSight()
    {
        Ray laserSightRay = new Ray(firePointCannon.transform.position, firePointCannon.transform.TransformDirection(Vector3.forward) * 1000);
        RaycastHit hit;
        if(Physics.Raycast(laserSightRay, out hit))
        {
            lr.SetPosition(0, laserSightRay.GetPoint(0));
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(0, laserSightRay.GetPoint(0));
            lr.SetPosition(1, laserSightRay.GetPoint(1) + firePointCannon.transform.TransformDirection(Vector3.forward) * 6);
        }
        
    }

    void CheckMachineGunReload()
    {
        if (machineGunAmmo == 0)
        {
            machineGunReloadTimer += Time.deltaTime;
            if (machineGunReloadTimer >= machineGunReloadTime)
            {
                machineGunAmmo = 30;
                machineGunReloadTimer = 0f;
            }
        }
    }
}
