using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneCreator : MonoBehaviour
{
    public static SceneCreator i;
    void Awake()
    {
        //debug
        if (i != null)
        {
            Debug.LogError("More than one SceneCreator in scene");
            return;
        }
        i = this;
    }

    [Header("Global")]
    public int playerLives = 3;
    public bool win = false;
    public bool lose = false;
    public bool isBossSpawned = false;
    public bool isBossDead = false;
    public bool fourthwave = false;
    public GameObject[] allNormalEnemies;
    [SerializeField] private GameObject cubeList;
    [Header("Things to spawn")]
    [SerializeField] private GameObject[] grasses = new GameObject[5];
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject bedRock;
    [SerializeField] private GameObject stonePillar;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy11;
    [SerializeField] private GameObject enemy12;
    [SerializeField] private GameObject enemy13;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject spawnVFX1;

    [SerializeField] private GameObject real_player_gameObject;
    [SerializeField] private GameObject real_boss_gameObject;

    public GameObject[] spawnPoints;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public int[,] gridToDelete;
    public int[,] bedRockPos;

    private int bedrockNum = 1;

    private void Start()
    {
        gridToDelete = new int[12, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 },
            { 0, 15 }, { 0, 16 }, { 1, 16 },
            { 15, 0 }, { 16, 0 }, { 16, 1 },
            { 16, 15 }, { 15, 16 }, { 16, 16 }
        };

        bedRockPos = new int[4, 2]
        {
            { 2, 2 }, { 2, 3 }, { 3, 2 }, { 3, 3 }
        };

        StartCoroutine(CreateGrid());
        StartCoroutine(CreatePillars());
        StartCoroutine(ReleasePlayer());
        StartCoroutine(TestReleaseEnemy0());
        StartCoroutine(TestReleaseEnemy1());
        StartCoroutine(TestReleaseEnemy2());
        StartCoroutine(TestReleaseEnemy3());
    }
    private void Update()
    {
        allNormalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (playerLives == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        if (playerLives == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        if (playerLives == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        if(fourthwave && !isBossSpawned && allNormalEnemies.Length == 0)
        {
            SpawnBoss();
            isBossSpawned = true;
            StartCoroutine(TestReleaseEnemy4());
        }
        
        if (playerLives == 0)
        {
            lose = true;
        }
        if(isBossSpawned && isBossDead && allNormalEnemies.Length == 0)
        {
            win = true;
        }
    }
    public void Test()
    {
        StartCoroutine(RespawnPlayer());
    }
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);
        playerLives--;
        if (playerLives > 0)
        {
            AudioManager.instance.Play(SoundList.Spawn);
            GameObject newPlayer = Instantiate(player, new Vector3(2.5f, 1f, -2.5f), Quaternion.identity);
            newPlayer.GetComponent<PlayerStat>().canvasFader.sec = 0;
            real_player_gameObject = newPlayer;

            GameObject newSpawnVFX = Instantiate(spawnVFX1, real_player_gameObject.transform.position, Quaternion.identity);
            Destroy(newSpawnVFX, 4f);

            
        }
        yield return new WaitForSeconds(0.5f);
        if (allNormalEnemies.Length > 0)
        {
            for (int i = 0; i < allNormalEnemies.Length; i++)
            {
                Debug.Log("AAAA");
                allNormalEnemies[i].GetComponent<EnemyBehaviour>().playerTarget = GameObject.FindGameObjectWithTag("Player");

            }
        }
        if(real_boss_gameObject)
        {
            real_boss_gameObject.GetComponent<BossBehaviour>().playerTarget = GameObject.FindGameObjectWithTag("Player");
        }
    }
    public void SpawnBoss()
    {
        AudioManager.instance.Play(SoundList.Spawn);
        GameObject newBoss = Instantiate(enemy2, new Vector3(13.5f, 2f, -13.5f), Quaternion.identity);
        real_boss_gameObject = newBoss;

        GameObject newSpawnVFX = Instantiate(spawnVFX1, newBoss.transform.position, Quaternion.identity);
        Destroy(newSpawnVFX, 4f);
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
                    int seed = Random.Range(0, 5);
                    GameObject newBlock = Instantiate(grasses[seed], cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);

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
                    
                    if (bedrockNum == 1)
                    {
                        //Quaternion initialRot = newBedRock.transform.rotation;
                        //newBedRock.transform.rotation = Quaternion.Euler(0, 270, 0);
                        //newBedRock.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 270, 0);
                        newBedRock.transform.GetChild(0).rotation = Quaternion.Euler(0, 270, 0);
                    }
                    if (bedrockNum == 2)
                    {
                        newBedRock.transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
                    }
                    if (bedrockNum == 3)
                    {
                        newBedRock.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (bedrockNum == 4)
                    {
                        newBedRock.transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                    }
                    x++;
                    y--;
                    bedrockNum++;
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
                int seed = Random.Range(0, 5);
                GameObject newBlock = Instantiate(grasses[seed], cubeList.transform.position + new Vector3(x, 0, -y), Quaternion.identity);

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

        Instantiate(stonePillar, new Vector3(1, 2f, -5), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(5, 2f, -1), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(11, 2f, -1), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(15, 2f, -5), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(1, 2f, -11), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(5, 2f, -15), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(11, 2f, -15), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(15, 2f, -11), Quaternion.identity);

        Instantiate(stonePillar, new Vector3(5, 2f, -9), Quaternion.identity);
        Instantiate(stonePillar, new Vector3(11, 2f, -6), Quaternion.identity);

        //for (int i = 0; i < 6; i++)
        //{
        //    bool canSpawnPillar = false;
        //    int randomX = Random.Range(1, 17);
        //    int randomY = Random.Range(1, 17);
        //    // Check if there is anything exists / pillar exists
        //    if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //    {
        //        canSpawnPillar = true;
        //    }
        //    if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Pillar")))
        //    {
        //        canSpawnPillar = false;
        //    }
        //    if (Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Player")))
        //    {
        //        canSpawnPillar = false;
        //    }
        //    if (!Physics.Raycast(new Vector3(randomX, 100, -randomY), Vector3.down, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //    {
        //        canSpawnPillar = false;
        //    }
        //    // Check if bedrock exists
        //    for (int a = 0; a < 4; a++)
        //    {
        //        if (randomX == bedRockPos[a, 0] && randomY == bedRockPos[a, 1])
        //        {
        //            canSpawnPillar = false;
        //        }
        //    }
        //    if (canSpawnPillar)
        //    {
        //        Debug.Log("Pillar " + randomX + " " + randomY);
        //        GameObject newStonePillar = Instantiate(stonePillar, new Vector3(randomX, 6f, -randomY), Quaternion.identity);
        //    }
        //}
    }
    IEnumerator ReleasePlayer()
    {
        yield return new WaitForSeconds(0.08f * 33f);
        AudioManager.instance.Play(SoundList.Spawn);
        GameObject newPlayer = Instantiate(player, new Vector3(2.5f, 1f, -2.5f), Quaternion.identity);
        real_player_gameObject = newPlayer;

        GameObject newSpawnVFX = Instantiate(spawnVFX1, real_player_gameObject.transform.position, Quaternion.identity);
        Destroy(newSpawnVFX, 4f);
    }

    IEnumerator TestReleaseEnemy0()
    {
        yield return new WaitForSeconds(0.08f * 33f + 7f);
        for (int i = 0; i < spawnPoints.Length - 2; i++)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].transform.position) <= 1.5f)
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy11, new Vector3(spawnPoints[5].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            else
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy11, new Vector3(spawnPoints[i].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            yield return new WaitForSeconds(Random.Range(0.7f, 0.9f));
        }
    }
    IEnumerator TestReleaseEnemy1()
    {
        yield return new WaitForSeconds(0.08f * 33f + 18f);
        for (int i = 0; i < spawnPoints.Length - 1; i++)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].transform.position) <= 1.5f)
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy11, new Vector3(spawnPoints[5].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            else
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy11, new Vector3(spawnPoints[i].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            yield return new WaitForSeconds(Random.Range(0.7f, 0.9f));
        }
    }
    IEnumerator TestReleaseEnemy2()
    {
        yield return new WaitForSeconds(0.08f * 33f + 32f);
        for (int i = 0; i < spawnPoints.Length - 1; i++)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].transform.position) <= 1.5f)
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy12, new Vector3(spawnPoints[5].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            else
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy12, new Vector3(spawnPoints[i].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));
        }
    }
    IEnumerator TestReleaseEnemy3()
    {
        yield return new WaitForSeconds(0.08f * 33f + 52f);
        for (int i = 0; i < spawnPoints.Length - 1; i++)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].transform.position) <= 1.5f)
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy13, new Vector3(spawnPoints[5].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            else
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy13, new Vector3(spawnPoints[i].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }
        fourthwave = true;
    }
    IEnumerator TestReleaseEnemy4()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 2; i < spawnPoints.Length - 1; i++)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].transform.position) <= 1.5f)
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy12, new Vector3(spawnPoints[5].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            else
            {
                AudioManager.instance.Play(SoundList.Spawn);
                GameObject newEnemy = Instantiate(enemy12, new Vector3(spawnPoints[i].transform.position.x, 1f, spawnPoints[i].transform.position.z), Quaternion.identity);
                GameObject newSpawnVFX = Instantiate(spawnVFX1, newEnemy.transform.position, Quaternion.identity);
                Destroy(newSpawnVFX, 4f);
            }
            yield return new WaitForSeconds(Random.Range(1.4f, 1.9f));
        }
        fourthwave = true;
    }
}
