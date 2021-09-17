using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunAmmo : MonoBehaviour
{
    private GameObject firePoint;
    [SerializeField] private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }
    public void SetProperties(GameObject firePoint)
    {
        this.firePoint = firePoint;
    }
    public void CalculateHitPositionAndDestroyAmmo()
    {
        RaycastHit hit;
        Vector3 start = firePoint.transform.position;
        Vector3 end = firePoint.transform.TransformDirection(Vector3.forward);
        // If hit aything
        if (Physics.Raycast(start, end * 10, out hit))
        {
            AudioManager.instance.Play(SoundList.EnemyBeingHit);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            Ray ray = new Ray(start, end);
            lr.SetPosition(0, ray.GetPoint(0));
            lr.SetPosition(1, ray.GetPoint(1) + firePoint.transform.TransformDirection(Vector3.forward * 10));
        }
        Destroy(gameObject, 0.08f);
    }
}
