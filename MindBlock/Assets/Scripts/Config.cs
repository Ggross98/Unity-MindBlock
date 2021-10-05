
public class Config
{
    /*
    public const int CELL_WALL = 1, CELL_TRACE = 2, CELL_PLAYER = 3, CELL_GOAL = 4, BLANK = 0, END = -1;
    public const int ARROW_DOWN = 5, ARROW_LEFT = 6, ARROW_UP = 7, ARROW_RIGHT = 8;
    public const int CELL_EXPLODE = 9, CELL_STAR = -2;*/

    public enum DIRECTION { DOWN, LEFT, RIGHT, UP, NONE }

    public enum CELL_TYPE {

        WALL = 1, TRACE = 2, PLAYER = 3, GOAL = 4, BLANK = 0, END = -1, 
        /*ARROW_DOWN, ARROW_LEFT, ARROW_UP, ARROW_RIGHT,*/
        EXPLODE = 9, STAR = -2

    }

    public const float MOVE_CELL_INTERVAL = 0.1f;

    public const int MAX_ROW = 20, MAX_COLUMN = 10;

    public const int CELL_SIZE = 80;

}
