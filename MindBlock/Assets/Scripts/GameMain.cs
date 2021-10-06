using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Ggross.Template;


public class GameMain : SingletonMonoBehaviour<GameMain>
{
    private int currentLevel;
    public static int maxLevels = 5;

    /*
    public Sprite sprite_player, sprite_wall, sprite_goal, sprite_star, sprite_explode, sprite_stop;
    public Sprite sprite_arrow_down, sprite_arrow_up, sprite_arrow_left, sprite_arrow_right;
    */

    //private bool moving = false;
    private bool gaming = false, winning = false;

    private int stars = 0;

    [SerializeField] private Text msg;
    


    

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            currentLevel = 0;
        }
        Debug.Log(currentLevel);

        //mapPrefab = new int[column, row];

        //msg = GameObject.Find("TopMessage").GetComponent<Text>();
        msg.gameObject .SetActive (false);

        //mapInfo = testMap;
        //DownloadMapInfo();

        //mapCurrent = new int[column, row];

        //CreateGrid();

        //while (MapController.Instance == null) { }

        MapController.Instance.CreateCellObjects();
        MapController.Instance.DownloadMapData(MapData.GetMapData(currentLevel));

        gaming = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if(Input.GetKeyDown (KeyCode.Escape))
        {
            Quit();
        }

        if (winning)
        {
            if(Input.GetKeyDown (KeyCode.Space))
            {
                StartNextLevel();
            }

            return;
        };


        if (!gaming) return;

        /*
        if(playerPos == mapInfo .goalPos )
        {
            Win();
            return;
        }
        */

        #region 玩家移动
        if(Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MapController.Instance.MovePlayer(Config.DIRECTION.LEFT);
            //dir = DIRECTION.LEFT;StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown (KeyCode.DownArrow))
        {
            MapController.Instance.MovePlayer(Config.DIRECTION.DOWN);
            //dir = DIRECTION.DOWN; StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MapController.Instance.MovePlayer(Config.DIRECTION.RIGHT);
            //dir = DIRECTION.RIGHT; StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MapController.Instance.MovePlayer(Config.DIRECTION.UP);
            //dir = DIRECTION.UP; StartCoroutine(Move());
        }
        #endregion

    }

    public void Restart()
    {
        
        stars = 0;
        //moving = false;
        gaming = true;
        winning = false;
        msg.gameObject.SetActive(false);

        MapController.Instance.Restart();

    }
    public void Quit()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void Win()
    {
        gaming = false;
        winning = true;
        msg.gameObject.SetActive(true);

        //记录游戏进度
        WriteLevelInfo(currentLevel, (stars > 0) ? '2' : '1');
        WriteLevelInfo(currentLevel+1,  '1');

    }

    public void StartNextLevel()
    {
        currentLevel++;
        if (currentLevel >= maxLevels) return;
        else
        {
            //DownloadMapInfo();
            Restart();
        }
    }

    public void Dead()
    {
        gaming = false;
    }

    public void Star()
    {
        stars++;
    }
    

    public void WriteLevelInfo(int level, char state)
    {
        if(PlayerPrefs .HasKey("LevelInfo"))
        {
            char[] levels = PlayerPrefs.GetString("LevelInfo").ToCharArray();
            Debug.Log(levels.Length);
            if(level<levels.Length)
            {
                levels[level] = state;
                PlayerPrefs.SetString("LevelInfo", new string(levels));
            }
            else
            {
                Debug.Log("Already the last level!");
            }
            
        }
        else
        {
            Debug.LogError("Doesn't have the correct key!");
        }
    }
    
}
