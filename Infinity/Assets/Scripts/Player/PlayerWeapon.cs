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
    private float cannonTimer = 0f;
    [SerializeField] private float cannonCD = 2f;
    [Header("Machine gun")]
    [SerializeField] private GameObject machineGunBullet;
    private float machineGunTimer = 0f;
    [SerializeField] private float machineGunCD = 0.08f;
    [SerializeField] private int machineGunAmmo = 30;
    private float machineGunReloadTimer = 0f;
    [SerializeField] private float machineGunReloadTime = 4f;
    [Header("")]

    [Header("Particle effect")]
    [SerializeField] private GameObject cannon_VFX1;
    [SerializeField] private GameObject cannon_VFX2;
    [SerializeField] private GameObject machineGun_VFX;
    [SerializeField] private GameObject bolt_VFX;
    [SerializeField] private GameObject missile_VFX;
    [Header("Laser Sight")]
    [SerializeField] private LineRenderer lr;

    void Start()
    {
        playerWeaponSelection = GetComponent<PlayerWeaponSelection>();

        cannonTimer = cannonCD;
        machineGunTimer = machineGunCD;

        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }
    
    void Update()
    {
        cannonTimer += Time.deltaTime;
        machineGunTimer += Time.deltaTime;

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
                cannonShellScript.SetProperties(shellSpeed);

                cannonTimer = 0f;

                GameObject newVFX = Instantiate(cannon_VFX1, firePointCannon.transform.position, firePointCannon.transform.rotation);
                newVFX.transform.SetParent(firePointCannon.transform);
                Destroy(newVFX, 2f);
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
                machineGunAmmoScript.SetProperties(firePointMachineGun);
                machineGunAmmoScript.CalculateHitPositionAndDestroyAmmo();

                machineGunTimer = 0f;
                machineGunAmmo--;

                GameObject newVFX = Instantiate(machineGun_VFX, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                newVFX.transform.SetParent(firePointMachineGun.transform);
                Destroy(newVFX, 2f);
            }
        }

        // Bolt cannon
        if(playerWeaponSelection.GetCurrentWeaponID() == 3)
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
            lr.SetPosition(1, laserSightRay.GetPoint(1) + firePointCannon.transform.TransformDirection(Vector3.forward) * 10);
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
