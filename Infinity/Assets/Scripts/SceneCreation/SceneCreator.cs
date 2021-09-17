using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCreator : MonoBehaviour
{
    [SerializeField] private GameObject cubeList;
    [Header("Things to spawn")]
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject bedRock;
    [SerializeField] private GameObject stonePillar;
    [SerializeField] private GameObject player;

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
        StartCoroutine(CreatePillars());
        StartCoroutine(ReleasePlayer());
    }
    IEnumerator CreateGrid()
    {
        for (int i = 0; i < 17; i++)
        {
            int x = 0;
            int y = i;
            for (int j = 0; j < i + 1; j++)
            {
                bool isBedRock = false;
                for (int a= 0; a < 4; a++)
                {
                    if(x == bedRockPos[a,0] && y == bedRockPos[a,1])
                    {
                        isBedRock = true;
                    }
                }
                if(!isBedRock)
                {
                    GameObject newBlock = Instantiate(cube, cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);

                    GridProperties gridProperties = newBlock.GetComponent<GridProperties>();
                    gridProperties.blockCreator = GetComponent<SceneCreator>();
                    newBlock.transform.SetParent(cubeList.transform);
                    gridProperties.x = x;
                    gridProperties.y = y;
                    gridProperties.RunPositionCheck();
                    x++;
                    y--;
                }
                else if(isBedRock)
                {
                    GameObject newBedRock = Instantiate(bedRock, cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);

                    GridProperties gridProperties = newBedRock.GetComponent<GridProperties>();
                    gridProperties.blockCreator = GetComponent<SceneCreator>();
                    newBedRock.transform.SetParent(cubeList.transform);
                    gridProperties.x = x;
                    gridProperties.y = y;
                    gridProperties.RunPositionCheck();
                    x++;
                    y--;
                }
                
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
                gridProperties.blockCreator = GetComponent<SceneCreator>();
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

    IEnumerator CreatePillars()
    {
        yield return new WaitForSeconds(0.08f * 33f);
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("DFDF");
            bool canSpawnPillar = false;
            int randomX = Random.Range(1, 17);
            int randomY = Random.Range(1, 17);
            
            // Check if there is anything exists / pillar exists
            if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                canSpawnPillar = true;
            }
            if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Pillar")))
            {
                canSpawnPillar = false;
            }
            if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Player")))
            {
                canSpawnPillar = false;
            }
            if (!Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                canSpawnPillar = false;
            }
            // Check if bedrock exists
            for (int a = 0; a < 4; a++)
            {
                if (randomX == bedRockPos[a, 0] && randomY == bedRockPos[a, 1])
                {
                    canSpawnPillar = false;
                }
            }
            if (canSpawnPillar)
            {
                Debug.Log("Pillar " + randomX + " " + randomY);
                GameObject newStonePillar = Instantiate(stonePillar, new Vector3(randomX, 6f, -randomY), Quaternion.identity);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator ReleasePlayer()
    {
        yield return new WaitForSeconds(0.08f * 33f);
        GameObject newPlayer = Instantiate(player, new Vector3(6.5f, 6f, -6.5f), Quaternion.identity);
    }
}
