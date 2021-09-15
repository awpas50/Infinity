using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProperties : MonoBehaviour
{
    public int x;
    public int y;

    public SceneCreator blockCreator;

    private float rotationY = 45;
    private float scaleX = 0.1f;
    private float scaleY = 0.1f;
    private float scaleZ = 0.1f;

    private bool isFinishTranformation;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        // Create a awesome effect.
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        transform.localEulerAngles = new Vector3(transform.rotation.x, rotationY, transform.rotation.z);
        if ((scaleX < 1 || rotationY > 0) && !isFinishTranformation)
        {
            rotationY -= Time.deltaTime * 90f;
            scaleX += Time.deltaTime * 1.5f;
            scaleY += Time.deltaTime * 1.5f;
            scaleZ += Time.deltaTime * 1.5f;
        }
        else if((scaleX >= 1 && rotationY <= 0) && !isFinishTranformation)
        {
            scaleX = 1;
            scaleY = 1;
            scaleZ = 1;
            rotationY = 0;
            isFinishTranformation = true;
        }
    }
    public void RunPositionCheck()
    {
        for (int a = 0; a < 12; a++)
        {
            if (x == blockCreator.gridToDelete[a, 0] && y == blockCreator.gridToDelete[a, 1])
            {
                Destroy(gameObject);
            }
        }
    }
}
