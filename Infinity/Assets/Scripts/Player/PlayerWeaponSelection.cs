using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSelection : MonoBehaviour
{
    [SerializeField] private int currentWeapon = 1;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeapon = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentWeapon = 5;
        }

        Debug.Log(currentWeapon);
    }

    public int GetCurrentWeaponID()
    {
        return currentWeapon;
    }
}
