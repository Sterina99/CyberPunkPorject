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
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;
    public GameObject cornerWallTiles;

    private Transform boardHolder;

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
        LayOutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
