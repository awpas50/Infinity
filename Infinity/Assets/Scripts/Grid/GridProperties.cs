using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProperties : MonoBehaviour
{
    public int x;
    public int y;

    public BlockCreator blockCreator;

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
