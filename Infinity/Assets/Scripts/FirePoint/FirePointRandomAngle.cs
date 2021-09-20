using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointRandomAngle : MonoBehaviour
{
    public void ResetFirePointAngle()
    {
        transform.localEulerAngles = new Vector3(
            0,
            0,
            0
        );
    }
    public void RandomProjectile()
    {
        transform.localEulerAngles = new Vector3(
            0,
            Random.Range(-10f, 10f),
            0
        );
    }
}
