using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Ggross.Template;

public class MapController : SingletonMonoBehaviour<MapController>
{
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private Transform grid;

    //private Config.CELL_TYPE[,] currentMap;
    private CellObject[,] cellList;

    private MapData mapData;

    //private List<CellObject> cellObjects;

    private Vector2Int playerPos;

    [HideInInspector] public bool moving = false;
    [HideInInspector] public Config.DIRECTION dir = Config.DIRECTION.NONE;

    private void Awake()
    {
        if(cellPrefab == null || grid == null)
        {
            Debug.Log("Game objects lost!");
        }
    }

    public Config.CELL_TYPE GetCellType(Vector2Int pos) {
        return Config.CELL_TYPE.BLANK;
    }

    public void ChangeCellType(CellObject obj, Config.CELL_TYPE type)
    {
        obj.type = type;
        obj.image.sprite = ImageGetter.Instance.GetSprite(type);
        //obj.button.interactable = false;

        switch (type)
        {
            case Config.CELL_TYPE.BLANK:
                obj.image.color = new Color(0, 0, 0, 0.2f);
                break;

            case Config.CELL_TYPE.PLAYER:
                obj.image.color = new Color(106f / 255f, 90f / 255f, 205f / 255f, 1);
                //obj.button.colors = cb;
                break;

            case Config.CELL_TYPE.WALL:
                obj.image.color = new Color(70f / 255f, 130f / 255f, 180f / 255f, 1);
                break;
            case Config.CELL_TYPE.GOAL:
                obj.image.color = Color.yellow;
                break;
            case Config.CELL_TYPE.TRACE:
                obj.image.color = new Color(255f / 255f, 228f / 255f, 180f / 255f, 1);
                break;
            /*case ARROW_DOWN:
                obj.image.sprite = sprite_arrow_down;
                obj.image.color = new Color();
                break;
            case ARROW_LEFT:
                obj.image.sprite = sprite_arrow_left;
                obj.image.color = new Color();
                break;
            case ARROW_UP:
                obj.image.sprite = sprite_arrow_up;
                obj.image.color = new Color();
                break;
            case ARROW_RIGHT:
                obj.image.sprite = sprite_arrow_right;
                obj.image.color = new Color();
                break;*/
            case Config.CELL_TYPE.END:
                obj.image.color = Color.red;
                break;

            case Config.CELL_TYPE.STAR:
                obj.image.color = Color.yellow;
                break;

        }


    }

    public void MovePlayer(Config.DIRECTION dir)
    {
        this.dir = dir;

        StartCoroutine(MoveCoroutine());
    }

    /// <summary>
    /// 生成网格中所有方块对象
    /// </summary>
    public void CreateCellObjects()
    {
        //删除已有物体
        var count = grid.childCount;
        for(int i = 0; i < count; i++)
        {
            Destroy(grid.GetChild(0).gameObject);
        }

        cellList = new CellObject[Config.MAX_ROW, Config.MAX_COLUMN];

        for (int i = 0; i < Config.MAX_ROW; i++)
        {
            for (int j = 0; j < Config.MAX_COLUMN; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, grid);
                newCell.name = new Vector2(i, j) + "";

                CellObject cs = newCell.GetComponent<CellObject>();
                cellList[i, j] = cs;
                cs.SetPosition(i, j);
                //ChangeCellType(cs, mapPrefab[i, j]);
            }
        }

        //UploadCurrentMapData();
    }

    private bool IsNextCellOutOfRange(Vector2Int pos)
    {
        int x = pos.x, y = pos.y;

        return ((dir == Config.DIRECTION.DOWN && y <= 0)
            || (dir == Config.DIRECTION.UP && y >= mapData.column - 1)
            || (dir == Config.DIRECTION.LEFT && x <= 0)
            || (dir == Config.DIRECTION.RIGHT && x >= mapData.row - 1));
    }

    public CellObject GetNextCellObject()
    {
        if (dir == Config.DIRECTION.NONE) return null;

        if (IsNextCellOutOfRange(playerPos)) return null;

        int x = playerPos.x;
        int y = playerPos.y;
        if (dir == Config.DIRECTION.DOWN) return cellList[x, y - 1];
        else if (dir == Config.DIRECTION.UP) return cellList[x, y + 1];
        else if (dir == Config.DIRECTION.LEFT) return cellList[x - 1, y];
        else return cellList[x + 1, y];

    }

    public Config.CELL_TYPE GetNextCellType()
    {
        if (dir == Config.DIRECTION.NONE) return Config.CELL_TYPE.PLAYER;

        var obj = GetNextCellObject();
        if (obj == null) return Config.CELL_TYPE.END;
        else return obj.type;

    }

    public void MoveOneCell(CellObject next)
    {
        CellObject player = cellList[playerPos.x, playerPos.y];
        
        ChangeCellType(player, Config.CELL_TYPE.TRACE);
        ChangeCellType(next, Config.CELL_TYPE.PLAYER);

        playerPos = next.pos;
        //UploadCurrentMapData();
    }

    /*
    private void UploadCurrentMapData()
    {
        for (int i = 0; i < mapData.row; i++)
        {
            for (int j = 0; j < mapData.column; j++)
            {
                CellObject cs = cellList[i, j];
                ChangeCellType(cs, mapData.map[i, j]);
                //currentMap[i, j] = cs.type;
            }

        }
    }*/


    /// <summary>
    /// 加载地图数据
    /// </summary>
    /// <param name="md">地图数据对象</param>
    public void DownloadMapData(MapData md)
    {

        mapData = md;

        for(int i = 0; i < mapData.row; i++)
        {
            for(int j = 0; j< mapData.column; j++)
            {
                //currentMap[i, j] = mapData.map[i, j];
                Debug.Log(i + "," + j);
                ChangeCellType(cellList[i, j], mapData.map[i, j]);
            }
        }
        
        playerPos = mapData.startPos;

    }

    public void Restart()
    {
        DownloadMapData(mapData);
    }

    private IEnumerator MoveCoroutine()
    {
        moving = true;
        CellObject next = GetNextCellObject();

        while (next != null && moving)
        {
            switch (next.type)
            {
                case Config.CELL_TYPE.STAR:
                    //stars++;
                    MoveOneCell(next);
                    next = GetNextCellObject();
                    break;

                case Config.CELL_TYPE.BLANK:
                    MoveOneCell(next);
                    next = GetNextCellObject();
                    break;

                case Config.CELL_TYPE.GOAL:

                    ChangeCellType(cellList[playerPos.x, playerPos.y], Config.CELL_TYPE.TRACE);
                    playerPos = next.pos;
                    moving = false;
                    break;

                default:
                    moving = false;
                    break;
            }

            yield return new WaitForSeconds(Config.MOVE_CELL_INTERVAL);
        }
        dir = Config.DIRECTION.NONE;
        moving = false;

        //走到界面边缘时死掉
        if (next == null)
        {
            ChangeCellType(cellList[playerPos.x, playerPos.y], Config.CELL_TYPE.END);

            //gaming = false;
        }

        yield return null;
    }

}
