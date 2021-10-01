using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject buttonGroups;
    public GameObject buttonPrefab;

    public Sprite star_light, star_black;

    public const int maxLevels = 5;

    public char[] levels;

    public void Start()
    {
        levels = new char[maxLevels];
        
        if(PlayerPrefs.HasKey("LevelInfo"))
        {
            levels = PlayerPrefs.GetString("LevelInfo").ToCharArray ();

            if(levels.Length < maxLevels)
            {
                for(int i = levels.Length; i < maxLevels; i++)
                {
                    levels[i]= '0';
                }
            }
        }
        else
        {
            //levels = new char[maxLevels ];
            for(int i = 0; i < maxLevels; i++)
            {
                levels[i] = '0';
            }
            
            
        }

        if (levels[0].Equals ('0'))
        {
            levels[0] = '1';
        }

        CreateButtons();
        

        PlayerPrefs.SetString("LevelInfo", new string (levels));
        string show = PlayerPrefs.GetString("LevelInfo");
        Debug.Log(show);

    }

    public void CreateButtons()
    {
        for (int i = 0; i < maxLevels ; i++)
        {
            GameObject g = Instantiate(buttonPrefab, buttonGroups.transform);
            g.name = "level" + (i + 1) + "";
            
            LevelButton levelButton = g.GetComponent<LevelButton>();
            levelButton.level = i;

            levelButton.rt.anchoredPosition = new Vector2(80 * (i % 20), i / 20 * 100);
            levelButton.text.text = (i + 1) + "";

            char j = levels[i];

            if (j.Equals('0'))
            {
                levelButton.button.interactable = false;
            }
            else
            {
                levelButton.button.interactable = true;
            }

            if (j.Equals('2'))
            {
                levelButton.image.sprite = star_light;
            }
            else
            {
                levelButton.image.sprite = star_black;
            }
            
            levelButton.GetComponent<Button>().onClick.AddListener(

                    delegate {
                        EnterStage(levelButton);
                    }

                );

        }

    }
    



    public void EnterStage(LevelButton  lb)
    {
        int i = lb.level;
        PlayerPrefs.SetInt("CurrentLevel", i);
        Debug.Log(PlayerPrefs.GetInt("CurrentLevel"));

        SceneManager.LoadScene("GameScene");
    }
}
