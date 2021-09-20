using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyStat enemyStat;
    public Image healthBar;

    void Update()
    {
        if(enemyStat.HP > enemyStat.initial_HP)
        {
            healthBar.fillAmount = 1;
        }
        else if (enemyStat.HP < 0)
        {
            healthBar.fillAmount = 0;
        }
        else
        {
            healthBar.fillAmount = (float)enemyStat.HP / (float)enemyStat.initial_HP;
        }
    }
}
