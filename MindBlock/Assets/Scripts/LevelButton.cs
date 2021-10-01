using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int level;
    public Button button;
    public Text text;
    public Image image;
    public RectTransform rt;

    void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        image = transform.GetChild(1).GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    public LevelButton (int i)
    {
        level = i;
    }
}
