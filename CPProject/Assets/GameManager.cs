using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public int chipInstalled = 0;

   
    [SerializeField] Player player;
    public List<GameObject> enemies;
    private GameObject levelImage;
    [SerializeField] GameObject mainMenu;
    public bool doingSetup=true;
    

    public static GameManager Instance()
    {
        return instance;
    }
    public BoardManager boardScript;

    [SerializeField] int level = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    //    level = 1;
        DontDestroyOnLoad(this);
        
        
       
       
      //  InitGame();
    }
    public void InitGame()
    {
        Debug.Log(level);

        mainMenu = GameObject.Find("MainMenu");
        if (mainMenu != null)
        {
            mainMenu.gameObject.SetActive(false);
        }
     
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
     //   if (boardScript == null)
       //     boardScript = GameObject.Find("PlatHolder").GetComponent<BoardManager>();
        enemies.Clear();
        if (player == null) player = GameObject.Find("Player").GetComponent<Player>();
        player.gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
        boardScript.PlatformSetup(level, chipInstalled);
        level += 1;
        Debug.Log(level);
        //    boardScript.SetUpScene(level);
    }
    // Start is called before the first frame update
   public void GameOver()
    {
       // enabled = false;
      //  level = 1;
        chipInstalled = 0;
        mainMenu.gameObject.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        level = 1;
        NextLevel();
    }
    public void NextLevel()
    {    
        if(level<=3)
            InitGame();
        else
        {
            ExitGame();
            Debug.Log("Done");
        }
    }
}
