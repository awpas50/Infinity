using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundList
{
    MachineGunSound1,
    MachineGunSound2,
    MachineGunSound3,
    CannonSound1,
    BoltCannonSound1,
    OutOfAmmo,
    SwitchWeapon,
    PlayerBeingHit,
    EnemyBeingHit
}

[System.Serializable]
public class Sound
{
    // as we are going to add audio in the AudioManager gameObject, we need a reference
    public AudioClip clip;
    public SoundList soundList;
    [HideInInspector] public AudioSource source;
    [HideInInspector] public float originalVolume;
    [Range(0f, 2f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;
    // determine 2D or 3D sound.
    [Range(0f, 1f)] public float reverbZoneMix;
}

