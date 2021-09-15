using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSelection : MonoBehaviour
{
    [SerializeField] private int currentWeapon = 1;
    private int previousWeapon = -1;

    private void Start()
    {
        currentWeapon = 1;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.instance.Play(SoundList.SwitchWeapon);
            currentWeapon = 1;
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.instance.Play(SoundList.SwitchWeapon);
            currentWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.instance.Play(SoundList.SwitchWeapon);
            currentWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.instance.Play(SoundList.SwitchWeapon);
            currentWeapon = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AudioManager.instance.Play(SoundList.SwitchWeapon);
            currentWeapon = 5;
        }

        Debug.Log(currentWeapon);
    }

    public int GetCurrentWeaponID()
    {
        return currentWeapon;
    }
}
