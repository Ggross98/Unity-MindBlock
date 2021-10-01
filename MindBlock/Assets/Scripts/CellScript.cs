using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    public int x, y;
    public Vector2Int pos;

    public int type;

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

        image.GetComponent<RectTransform>().anchoredPosition = new Vector2(x*80, y*80);
    }

    public void SetPosition(Vector2Int p)
    {
        SetPosition(p.x, p.y);
    }
}
