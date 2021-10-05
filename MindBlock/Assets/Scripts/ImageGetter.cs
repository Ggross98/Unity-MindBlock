using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Ggross.Template;

public class ImageGetter: SingletonMonoBehaviour<ImageGetter>
{
    [SerializeField]
    private Sprite sprite_player, sprite_wall, sprite_goal, sprite_star, sprite_explode, sprite_end, sprite_trace;
    //public Sprite sprite_arrow_down, sprite_arrow_up, sprite_arrow_left, sprite_arrow_right;
    
    public Sprite GetSprite(Config.CELL_TYPE type)
    {
        switch (type)
        {
            case Config.CELL_TYPE.BLANK:
                return sprite_wall;
            case Config.CELL_TYPE.END:
                return sprite_end;
            case Config.CELL_TYPE.EXPLODE:
                return sprite_explode;
            case Config.CELL_TYPE.GOAL:
                return sprite_goal;
            case Config.CELL_TYPE.PLAYER:
                return sprite_player;
            case Config.CELL_TYPE.TRACE:
                return sprite_trace;
            case Config.CELL_TYPE.STAR:
                return sprite_star;
        }

        return sprite_wall;
    }
}
