using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    public float sec = 8f;
    float t = 0;

    private Vector3 scaleChange;
    void Start()
    {
        scaleChange = new Vector3(-0.001f, -0.001f, -0.001f);
    }
    
    void Update()
    {
        t += Time.deltaTime;
        if(t >= sec)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale += scaleChange;
            }
            if(transform.localScale.x <= 0)
            {
                transform.localScale = new Vector3(0, 0, 0);
            }
            
        }
    }
}
