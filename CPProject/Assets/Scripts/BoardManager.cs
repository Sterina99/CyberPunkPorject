using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int min;
        public int max;
        public Count(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
    [Header("Dimesions")]
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count chipCount = new Count(0, 2);

    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] chipTiles;
    public GameObject[] enemyTiles;
    public GameObject[] enemyRobotTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject cornerWallTiles;
    [SerializeField] GameObject trampolinePrefab;

    private Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();


    private int platsNumber=20;
    void InitialiseList()
    {
        gridPositions.Clear();

        for (int i = 1; i < columns - 1; i++)
        {
            for (int j = 1; j < rows - 1; j++)
            {
                gridPositions.Add(new Vector3(i, j, 0f));
            }
        }

    }

    void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        bool isCorner = false;
        int cornerRot = 0;
        for (int i = -1; i < columns + 1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
                isCorner = false;
                GameObject toInstatiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (i == -1 || i == columns || j == -1 || j == rows)
                {
                    
                        toInstatiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstatiate, new Vector3(i, j, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
               
            }
        }
    }

   public void PlatformSetup(int level,int chipsNumber)
    {
        int min=0, max=100;
        float posX = 0.7f;
        float posY = -1;
        float incX = 0.7f;
        float incY = -1;
        bool canHoldEnemy = false;
        GameObject instance=null;
        GameObject prevInstance=null;
        GameObject toInstatiate = null;
        for (int i = 0; i < platsNumber; i++)
        {
            canHoldEnemy = false;
            if (level < 0)
            {
                toInstatiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                level--;
            }
            else
            {
                toInstatiate = wallTiles[Random.Range(0, wallTiles.Length)];
            }
           
            for (int j = 1; j < 3; j++)
            {

                if (toInstatiate.name == "Wall3" || toInstatiate.name == "Wall4" || toInstatiate.name == "Wall5")
                {
                    if (prevInstance != null) {
                        incY = 0;
                    if (prevInstance.name.Contains("Wall3"))
                        {
                        incY = 1.5f;
                    }else if (prevInstance.name.Contains("Wall4"))
                        {
                        incY = 2f;

                    }else if (prevInstance.name.Contains("Wall5"))
                        {
                        incY = 2.5f;
                    }
                    }
                Instantiate(trampolinePrefab, new Vector3(posX, posY + incY, 0f), Quaternion.identity);
                }
                    

                incY = Random.Range(1, 2) / 2f;
               
                incX = Random.Range(1, 3) / 2f;
               
                if (prevInstance != null) { 
                if (prevInstance.name.Contains("Wall3"))
                {
                    incY = 1.5f;
                }
                else if (prevInstance.name.Contains("Wall4"))
                    {
                    incY = 2f;

                }
                else if (prevInstance.name.Contains("Wall5"))
                    {
                    incY = 2.5f;
                }else if (prevInstance.name.Contains("Floor3"))
                    {
                    incX = 1.5f;
                        canHoldEnemy = true;
                        Debug.Log(canHoldEnemy);
                      
                }
                else if (prevInstance.name.Contains("Floor4"))
                    {
                    incX = 2f;
                        canHoldEnemy = true;
                        Debug.Log(canHoldEnemy);

                    }
                else if (prevInstance.name.Contains("Floor5"))
                    {
                    incX = 2.5f;
                        canHoldEnemy = true;
                        Debug.Log(canHoldEnemy);
                    }
                   
                }
                posY += incY;
                posX += incX;
                prevInstance = instance;
                if (canHoldEnemy)
                {
                    Debug.Log("SpawnEnemy");
                    if(Random.Range(min,max)>50+chipsNumber*2)//U are more robot than human, so you fight robot
                    Instantiate(enemyRobotTiles[Random.Range(0, enemyRobotTiles.Length)], new Vector3(posX, posY+0.5f, 0f), Quaternion.identity);
                    else
                    Instantiate(enemyTiles[Random.Range(0, enemyTiles.Length)], new Vector3(posX, posY+0.5f, 0f), Quaternion.identity);

                }
                instance = Instantiate(toInstatiate, new Vector3(posX, posY, 0f), Quaternion.identity);

               
            }
            
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayOutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetUpScene(int level)
    {
        BoardSetUp();
        InitialiseList();
        LayOutObjectAtRandom(wallTiles, wallCount.min, wallCount.max);
     //   if (chipTiles != null || chipTiles.Length>0)
    //    LayOutObjectAtRandom(chipTiles, chipCount.min, chipCount.max);
        int enemyCount = (int)Mathf.Log(level, 2f);
      //  if(GameManager.instance.chipInstalled>= 10)
        LayOutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //else
      //  LayOutObjectAtRandom(enemyRobotTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
