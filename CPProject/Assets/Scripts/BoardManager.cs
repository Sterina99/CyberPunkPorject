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
    public GameObject boss;
    public GameObject[] floorTiles;
    public GameObject[] chipTiles;
    public GameObject[] enemyTiles;
    public GameObject[] enemyRobotTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject cornerWallTiles;
    [SerializeField] GameObject trampolinePrefab;

    [SerializeField] GameObject platHolder;

    private List<Vector3> gridPositions = new List<Vector3>();

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
      //  boardHolder = new GameObject("Board").transform;
   //     bool isCorner = false;
      //  int cornerRot = 0;
        for (int i = -1; i < columns + 1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
              //  isCorner = false;
                GameObject toInstatiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (i == -1 || i == columns || j == -1 || j == rows)
                {
                    
                        toInstatiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstatiate, new Vector3(i, j, 0f), Quaternion.identity);
          //      instance.transform.SetParent(boardHolder);
               
            }
        }
    }

    public void PlatformSetup(int level, int chipsNumber)
    {
        
        int min = 0, max = 100;
        int platNumber = 0;
        float posX = GameObject.Find("GroundCheck").GetComponent<Transform>().position.x;
        float posY = GameObject.Find("GroundCheck").GetComponent<Transform>().position.y;
        GameManager.instance.doingSetup = true;
        float incX = 0;
        float incY = -1;
        bool canHoldEnemy = false;
        GameObject instance = null;
        GameObject prevInstance = null;
        GameObject toInstatiateFirst = null;
        GameObject toInstatiateSecond = null;
        Debug.Log(level);

       if (level > 1)
                CleanLevel();
            
        
            platHolder = new GameObject("PlatHolder");
        for (int i = 0; i <= 12 + level * 2;i++)
        {
            
            canHoldEnemy = false;
            //Walls
            toInstatiateFirst = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
           
            //Floors
            toInstatiateSecond = wallTiles[Random.Range(0, wallTiles.Length)];
            if (i != 0)
            {
                //    if (level < 0)
                //     {
                //        level--;
                //    }
                //  else
                //  {
                //toInstatiate = wallTiles[Random.Range(0, wallTiles.Length)];
                //    }


                incY = Random.Range(1, 2) / 2f;

            incX = Random.Range(1, 3) / 2f;
            
          

            if (prevInstance != null) {
                if (prevInstance.name.Contains("Wall3"))
                {
                    incY = 1.5f;
               //     Instantiate(trampolinePrefab, new Vector3(posX, posY + incY, 0f), Quaternion.identity);
                }
                else if (prevInstance.name.Contains("Wall4"))
                {
                    incY = 2f;
              //      Instantiate(trampolinePrefab, new Vector3(posX, posY + incY, 0f), Quaternion.identity);
                }
                else if (prevInstance.name.Contains("Wall5"))
                {
                    incY = 2.5f;
              //      Instantiate(trampolinePrefab, new Vector3(posX, posY + incY, 0f), Quaternion.identity);
                }
                else if (prevInstance.name.Contains("Floor3"))
                {
                    incX = 1.5f;
                    
               

                }
                else if (prevInstance.name.Contains("Floor4"))
                {
                    incX = 2f;
                    
                 

                }
                else if (prevInstance.name.Contains("Floor5")) { 
                incX = 2.5f;
                    
                }
            
          
           
            }
       
                posY += incY;
                posX += incX;
                //end of the level
            if (i == 12 + level * 2 )
                {
                        instance = Instantiate(wallTiles[wallTiles.Length-1], new Vector3(posX, posY, 0f), Quaternion.identity);
                    instance.transform.SetParent(platHolder.transform);

                    if (level == 3)
                    {
                        GameManager.instance.enemies.Add( Instantiate(boss, new Vector3(posX, posY + 0.5f, 0f), Quaternion.identity));
                    }
                   instance= Instantiate(exit, new Vector3(posX+1.8f, posY+0.75f, 0f), Quaternion.identity);
                    instance.transform.SetParent(platHolder.transform);
                }               
            //middle fo the level
                 else if (Random.Range(0, 10) >= 2 && platNumber < level)
                  {
                 //   Debug.Log(platNumber);
                //    Debug.Log(level);
                        platNumber++;
                        instance = Instantiate(toInstatiateSecond, new Vector3(posX, posY, 0f), Quaternion.identity);
                    instance.transform.SetParent(platHolder.transform);

                    prevInstance = instance;
                    
                        if (Random.Range(min, max) > 50 + chipsNumber * 2)//U are more robot than human, so you fight robot
                            GameManager.instance.enemies.Add(Instantiate(enemyRobotTiles[Random.Range(0, enemyRobotTiles.Length)], new Vector3(posX, posY + 0.5f, 0f), Quaternion.identity));
                        else
                            GameManager.instance.enemies.Add(Instantiate(enemyTiles[Random.Range(0, enemyTiles.Length)], new Vector3(posX, posY + 0.5f, 0f), Quaternion.identity));      
                 }
                else
                {   
                   instance= Instantiate(trampolinePrefab, new Vector3(posX-0.5f, posY, 0f), Quaternion.identity);
                    instance.transform.SetParent(platHolder.transform);
                    instance = Instantiate(toInstatiateFirst, new Vector3(posX, posY, 0f), Quaternion.identity);
                    instance.transform.SetParent(platHolder.transform);

                    prevInstance = instance;
                     
                }
            }
            else
            {
                instance = Instantiate(toInstatiateSecond, new Vector3(posX, posY, 0f), Quaternion.identity);
                instance.transform.SetParent(platHolder.transform);
                prevInstance = instance;
                
            }
        }
     //   Debug.Log(GameManager.instance.enemies[GameManager.instance.enemies.Count - 1]);
        GameManager.instance.enemies[GameManager.instance.enemies.Count-1].GetComponent<EnemyController>().AssignChip();
        
    }

    private void CleanLevel()
    {
       foreach(GameObject enemy in GameManager.instance.enemies)
        {
            Destroy(enemy);
        }

        Destroy(platHolder.gameObject);
    
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
    void Update()
    {
        if(platHolder==null)
       platHolder = GameObject.Find("PlatHolder");  
    }

}
