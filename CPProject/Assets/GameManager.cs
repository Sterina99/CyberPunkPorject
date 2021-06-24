using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public int chipInstalled = 0;

    private List<Enemy> enemies;
    private GameObject levelImage;
    private bool doingSetup;

    public static GameManager Instance()
    {
        return instance;
    }
    public BoardManager boardScript;

    private int level = 3;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
        boardScript.PlatformSetup(level,chipInstalled);
      //  InitGame();
    }
    void InitGame()
    {
        enemies.Clear();
        boardScript.SetUpScene(level);
    }
    // Start is called before the first frame update
   public void GameOver()
    {
        enabled = false;
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }
}
