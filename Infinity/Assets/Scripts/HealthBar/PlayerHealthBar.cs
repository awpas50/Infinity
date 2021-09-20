using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerStat playerStat;
    public Image healthBar;

    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>();
    }
    void Update()
    {
        if(!healthBar)
        {
            healthBar.fillAmount = 0;
        }
        else if(healthBar)
        {
            if (playerStat.HP > playerStat.initial_HP)
            {
                healthBar.fillAmount = 1;
            }
            else if (playerStat.HP < 0)
            {
                healthBar.fillAmount = 0;
            }
            else
            {
                healthBar.fillAmount = (float)playerStat.HP / (float)playerStat.initial_HP;
            }
        }
    }
}
