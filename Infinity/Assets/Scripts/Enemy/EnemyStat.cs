using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private float initial_HP;
    public float HP;
    void Start()
    {
        initial_HP = HP;
    }
    
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Destroy(gameObject, 0.15f);
        }
    }
}
