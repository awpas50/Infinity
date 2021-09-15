using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShell : MonoBehaviour
{
    private float speed;
    
    // Called in PlayerShoot()
    public void SetProperties(float speed)
    {
        this.speed = speed;
    }

    private void Update()
    {
        Destroy(gameObject, 10f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
