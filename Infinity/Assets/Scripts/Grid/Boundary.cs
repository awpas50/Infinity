using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStat>().TakeDamage(100000);
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStat>().TakeDamage(100000);
        }
        if (other.gameObject.tag == "Pillar")
        {
            other.gameObject.GetComponent<PillarProperties>().TakeDamage(100000);
        }
    }
}
