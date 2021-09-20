using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float initial_HP;
    public float HP;
    public GameObject VFX;

    public CanvasFader canvasFader;

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            GameObject a = Instantiate(VFX, transform.position, Quaternion.identity);
            Destroy(a, 2f);
            int seed = Random.Range(0, 3);
            switch (seed)
            {
                case 0:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed1);
                    break;
                case 1:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed2);
                    break;
                case 2:
                    AudioManager.instance.Play(SoundList.ObjectDestroyed3);
                    break;
            }
            SceneCreator.i.Test();
            //StartCoroutine(SceneCreator.i.EnemyFindPlayer());
            Destroy(gameObject, 0.1f);
        }
    }
}
