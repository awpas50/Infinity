using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    [SerializeField] private GameObject cubeList;
    [Header("Things to spawn")]
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject bedRock;

    public int[,] gridToDelete;
    public int[,] bedRockPos;

    private void Start()
    {
        gridToDelete = new int[12, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 },
            { 0, 15 }, { 0, 16 }, { 1, 16 },
            { 15, 0 }, { 16, 0 }, { 16, 1 },
            { 16, 15 }, { 15, 16 }, { 16, 16 }
        };

        bedRockPos = new int[4, 2]
        {
            { 6, 6 }, { 6, 7 }, { 7, 6 }, { 7, 7 }
        };

        StartCoroutine(CreateGrid());
        DeleteGrid();
        
    }
    IEnumerator CreateGrid()
    {
        for (int i = 0; i < 17; i++)
        {
            int x = 0;
            int y = i;
            for (int j = 0; j < i + 1; j++)
            {
                GameObject newBlock = Instantiate(cube, cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);
                GridProperties gridProperties = newBlock.GetComponent<GridProperties>();
                gridProperties.blockCreator = GetComponent<BlockCreator>();
                newBlock.transform.SetParent(cubeList.transform);
                gridProperties.x = x;
                gridProperties.y = y;
                gridProperties.RunPositionCheck();
                x++;
                y--;
                //Debug.Log("(" + x + "," + y + ")");
            }
            yield return new WaitForSeconds(0.08f);
        }
        for (int i = 0; i < 17 - 1; i++)
        {
            int x = i + 1;
            int y = 17 - 1;
            for (int j = 0; j < 17 - 1 - i; j++)
            {
                GameObject newBlock = Instantiate(cube, cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);
                GridProperties gridProperties = newBlock.GetComponent<GridProperties>();
                gridProperties.blockCreator = GetComponent<BlockCreator>();
                newBlock.transform.SetParent(cubeList.transform);
                gridProperties.x = x;
                gridProperties.y = y;
                gridProperties.RunPositionCheck();
                x++;
                y--;
                Debug.Log("(" + x + "," + y + ")");
            }
            yield return new WaitForSeconds(0.08f);
        }
        // y,x
        //0. 00
        //1. 10 01
        //2. 20 11 02
        //3. 30 21 12 03
        //4. 31 22 13
        //5. 32 23
        //6. 33
        // ......
        yield return null;
    }

    void DeleteGrid()
    {
    }
}
