using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameMain : MonoBehaviour
{
    private int currentLevel;

    public static int row = 10, column = 20;
    public static int cellSize = 80;
    public static int maxLevels = 5;

    public const int CELL_WALL = 1, CELL_TRACE = 2, CELL_PLAYER = 3, CELL_GOAL = 4, BLANK = 0, END = -1;
    public const int ARROW_DOWN = 5, ARROW_LEFT = 6, ARROW_UP = 7, ARROW_RIGHT = 8;
    public const int CELL_EXPLODE = 9, CELL_STAR = -2;

    public GameObject cellPrefab;

    public Sprite sprite_player, sprite_wall, sprite_goal, sprite_star, sprite_explode, sprite_stop;
    public Sprite sprite_arrow_down, sprite_arrow_up, sprite_arrow_left, sprite_arrow_right;

    public MapInfo mapInfo;

    private int[,] mapPrefab,mapCurrent;
    private CellScript [,] cellList;

    private Vector2Int playerPos;
    private enum DIRECTION { DOWN,UP,LEFT,RIGHT,STOP };
    private DIRECTION dir = DIRECTION.STOP;

    private bool moving = false;
    private bool gaming = false, winning = false;

    private int stars = 0;

    private Text msg;

    //public MapInfo testMap = new MapInfo(m1);


    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        Debug.Log(currentLevel);

        mapPrefab = new int[column, row];

        msg = GameObject.Find("TopMessage").GetComponent<Text>();
        msg.gameObject .SetActive (false);

        //mapInfo = testMap;
        DownloadMapInfo();

        mapCurrent = new int[column, row];

        CreateGrid();

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

        if (moving) return;

        if(playerPos == mapInfo .goalPos )
        {
            Win();
            return;
        }

        if(Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = DIRECTION.LEFT;StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown (KeyCode.DownArrow))
        {
            dir = DIRECTION.DOWN; StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = DIRECTION.RIGHT; StartCoroutine(Move());
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            dir = DIRECTION.UP; StartCoroutine(Move());
        }

    }

    public void Restart()
    {
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                mapCurrent[i, j] = mapPrefab[i, j];
                ChangeCellType(cellList[i, j], mapPrefab[i, j]);
                playerPos = mapInfo.startPos;

            }
        }
        stars = 0;
        moving = false;
        gaming = true;
        winning = false;
        msg.gameObject.SetActive(false);

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
        WriteLevelInfo(currentLevel, (stars > 0) ? '2' : '1');
        WriteLevelInfo(currentLevel+1,  '1');

    }

    public void StartNextLevel()
    {
        currentLevel++;
        if (currentLevel >= maxLevels) return;
        else
        {
            DownloadMapInfo();
            Restart();
        }
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

    public IEnumerator Move()
    {
        moving = true;
        CellScript next = GetNextCell();
        while (next != null && moving)
        {

            switch(next.type)
            {
                case CELL_STAR:
                    stars++;
                    MoveOneCell(next);
                    next = GetNextCell();
                    break;
                case BLANK:
                    MoveOneCell(next);
                    next = GetNextCell();
                    break;

                case CELL_GOAL:
                    
                    ChangeCellType(GetPlayer(), CELL_TRACE);
                    playerPos = next.pos;
                    moving = false;
                    break;

                default:
                    moving = false;
                    break;
            }
            
            yield return new WaitForSeconds(0.02f);
        }
        dir = DIRECTION.STOP;
        moving = false;

        if(next == null)
        {
            ChangeCellType(cellList[playerPos.x, playerPos.y], END);
            gaming = false;
        }

        yield return null;
    }

    public CellScript GetPlayer()
    {
        return cellList[playerPos.x, playerPos.y];
    }


    public int GetNextCellType()
    {
        if(dir == DIRECTION.STOP)
        {
            return CELL_PLAYER;
        }
        int x = playerPos.x;
        int y = playerPos.y;

        if((dir == DIRECTION.DOWN && y<=0)
            ||(dir == DIRECTION.UP && y >= 9)
            ||(dir == DIRECTION.LEFT && x <= 0)
            ||(dir == DIRECTION.RIGHT && x >= 19))
        {
            return END;
        }

        if (dir == DIRECTION.DOWN) return mapCurrent[x, y - 1];
        if (dir == DIRECTION.UP) return mapCurrent[x, y + 1];
        if (dir == DIRECTION.LEFT) return mapCurrent[x-1, y];
        if (dir == DIRECTION.RIGHT) return mapCurrent[x+1, y];

        return END;
    }

    public CellScript GetNextCell()
    {

        if (dir == DIRECTION.STOP)
        {
            return null;
        }
        int x = playerPos.x;
        int y = playerPos.y;

        if ((dir == DIRECTION.DOWN && y <= 0)
            || (dir == DIRECTION.UP && y >= 9)
            || (dir == DIRECTION.LEFT && x <= 0)
            || (dir == DIRECTION.RIGHT && x >= 19))
        {
            return null;
        }

        if (dir == DIRECTION.DOWN) return cellList [x, y - 1];
        if (dir == DIRECTION.UP) return cellList[x, y + 1];
        if (dir == DIRECTION.LEFT) return cellList[x - 1, y];
        if (dir == DIRECTION.RIGHT) return cellList[x + 1, y];

        return null;
    }

    public void MoveOneCell(CellScript next)
    {
        CellScript player = cellList[playerPos.x, playerPos.y];

        ChangeCellType(player, CELL_TRACE );
        ChangeCellType(next, CELL_PLAYER);
        playerPos = next.pos;
        UploadCurrentMap();
    }


    public void CreateGrid()
    {
        cellList = new CellScript [column,row];

        for(int i = 0; i < column; i++)
        {
            for(int j = 0; j < row; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, GameObject .Find("Board").transform );
                newCell.name = new Vector2(i, j) + "";
                
                CellScript cs = newCell.GetComponent<CellScript>();

                cs.SetPosition(i, j);
                ChangeCellType(cs, mapPrefab[i, j]);

                cellList[i, j] = cs ;
            }
        }

        UploadCurrentMap();
    }

    public void UploadCurrentMap()
    {
        for(int i = 0;i<20; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                CellScript cs = cellList[i,j];
                mapCurrent[cs.x, cs.y] = cs.type;
            }
            
        }
    }

    public void DownloadMapInfo()
    {

        mapInfo = MapInfo.GetMapInfo(currentLevel);

        for(int i =0;i<mapInfo.map.Length; i++)
        {
            int x = i % 20;
            int y = 9 - (i / 20);

            mapPrefab[x, y] = mapInfo.map[i];
        }
        playerPos = mapInfo.startPos;
        
    }

    public void ChangeCellType(CellScript cs,int t)
    {
        cs.type = t;
        //cs.button.interactable = false;

        switch (t)
        {
            case BLANK:

                cs.image.sprite  = sprite_wall;
                cs.image.color = new Color(0,0,0,0.2f);
                break;

            case CELL_PLAYER:
                cs.image.sprite = sprite_player;
                cs.image.color = new Color(106f/255f,90f/255f,205f/255f,1);
                //cs.button.colors = cb;
                break;

            case CELL_WALL:
                cs.image.sprite = sprite_wall;

                cs.image.color = new Color(70f/255f, 130f/255f, 180f/255f,1);
                break;
            case CELL_GOAL:
                cs.image.sprite = sprite_player;

                cs.image.color = Color.yellow ;
                break;
            case CELL_TRACE:
                cs.image.sprite = sprite_wall;
                cs.image.color = new Color(255f / 255f, 228f / 255f, 180f / 255f, 1);
                break;
            case ARROW_DOWN:
                cs.image.sprite = sprite_arrow_down;
                cs.image.color = new Color();
                break;
            case ARROW_LEFT:
                cs.image.sprite = sprite_arrow_left;
                cs.image.color = new Color();
                break;
            case ARROW_UP:
                cs.image.sprite = sprite_arrow_up;
                cs.image.color = new Color();
                break;
            case ARROW_RIGHT:
                cs.image.sprite = sprite_arrow_right;
                cs.image.color = new Color();
                break;
            case END:
                cs.image.sprite = sprite_stop;
                cs.image.color = Color.red ;
                break;

            case CELL_STAR:
                cs.image.sprite = sprite_star;
                cs.image.color = Color.yellow ;
                break;

        }
    }

    
}
