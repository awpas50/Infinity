using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillarHealthBar : MonoBehaviour
{
    public PillarProperties pillarProperties;
    public Image healthBar;
    public CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup.alpha = 0;
    }

    void Update()
    {
        if(canvasGroup.alpha >= 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 0.2f;
        }
        if (pillarProperties.HP > pillarProperties.initial_HP)
        {
            healthBar.fillAmount = 1;
        }
        else if (pillarProperties.HP < 0)
        {
            healthBar.fillAmount = 0;
        }
        else
        {
            healthBar.fillAmount = (float)pillarProperties.HP / (float)pillarProperties.initial_HP;
        }
    }

    public void SetAlpha()
    {
        canvasGroup.alpha = 1;
    }
}
