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
            if (Input.GetMouseButtonDown(0) && cannonTimer >= cannonCD)
            {
                AudioManager.instance.Play(SoundList.CannonSound1);
                GameObject newShell = Instantiate(shell, firePointCannon.transform.position, firePointCannon.transform.rotation);
                CannonShell cannonShellScript = newShell.GetComponent<CannonShell>();
                cannonShellScript.SetProperties(shellSpeed);

                cannonTimer = 0f;
            }
        }
        // Machine Gun
        if (playerWeaponSelection.GetCurrentWeaponID() == 2)
        {
            firePointRandomAngle.RandomProjectile();
            if (Input.GetMouseButton(0))
            {
                if(machineGunTimer >= machineGunCD && machineGunAmmo > 0)
                {
                    AudioManager.instance.Play(SoundList.MachineGunSound2);
                    GameObject newBullet = Instantiate(machineGunBullet, firePointMachineGun.transform.position, firePointMachineGun.transform.rotation);
                    MachineGunAmmo machineGunAmmoScript = newBullet.GetComponent<MachineGunAmmo>();
                    machineGunAmmoScript.SetProperties(firePointMachineGun);
                    machineGunAmmoScript.CalculateHitPositionAndDestroyAmmo();

                    machineGunTimer = 0f;
                    machineGunAmmo--;
                }
            }
        }
    }

    void ActivateLaserSight()
    {
        Ray laserSightRay = new Ray(firePointCannon.transform.position, firePointCannon.transform.TransformDirection(Vector3.forward) * 1000);
        Debug.DrawRay(firePointCannon.transform.position, firePointCannon.transform.TransformDirection(Vector3.forward) * 10, Color.cyan);
        lr.SetPosition(0, laserSightRay.GetPoint(0));
        lr.SetPosition(1, laserSightRay.GetPoint(1) + firePointCannon.transform.TransformDirection(Vector3.forward) * 10);
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
