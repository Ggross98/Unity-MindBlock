using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellObject : MonoBehaviour
{
    public int x, y;
    public Vector2Int pos;

    public Config.CELL_TYPE type;

    public Image image;

    
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.pos = new Vector2Int(x, y);

        image.GetComponent<RectTransform>().anchoredPosition = new Vector2(x*Config.CELL_SIZE, y*Config.CELL_SIZE);
    }

    public void SetPosition(Vector2Int p)
    {
        SetPosition(p.x, p.y);
    }
}
