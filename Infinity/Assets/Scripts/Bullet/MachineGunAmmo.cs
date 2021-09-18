using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunAmmo : MonoBehaviour
{
    private GameObject firePoint;
    private GameObject machineGun_VFX_fly;
    private GameObject machineGun_VFX_hit;
    [SerializeField] private LineRenderer lr;
    RaycastHit hit;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }
    public void SetProperties(GameObject firePoint, GameObject machineGun_VFX_fly, GameObject machineGun_VFX_hit)
    {
        this.firePoint = firePoint;
        this.machineGun_VFX_fly = machineGun_VFX_fly;
        this.machineGun_VFX_hit = machineGun_VFX_hit;
    }
    public void CalculateHitPositionAndDestroyAmmo()
    {
        
        Vector3 start = firePoint.transform.position;
        Vector3 end = firePoint.transform.TransformDirection(Vector3.forward);
        // If hit aything
        if (Physics.Raycast(start, end * 100, out hit))
        {
            AudioManager.instance.Play(SoundList.EnemyBeingHit);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            Ray ray = new Ray(start, end);
            lr.SetPosition(0, ray.GetPoint(0));
            lr.SetPosition(1, ray.GetPoint(1) + firePoint.transform.TransformDirection(Vector3.forward * 100));

            Destroy(machineGun_VFX_hit);
        }
        Destroy(gameObject, 0.08f);
    }

    private void LateUpdate()
    {
        if(machineGun_VFX_fly)
        {
            machineGun_VFX_fly.transform.position += machineGun_VFX_fly.transform.forward * Time.deltaTime * 350f;
            Destroy(machineGun_VFX_fly, 0.08f);
        }
        if(machineGun_VFX_hit)
        {
            machineGun_VFX_hit.transform.position = Vector3.Lerp(transform.position, hit.point, 1f);
        }
        
    }
}
