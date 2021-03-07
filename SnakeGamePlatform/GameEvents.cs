using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using WMPLib;

namespace SnakeGamePlatform
{
    
    public class GameEvents:IGameEvents
    {
        //Define game variables here! for example...
        GameObject[] snakeBody;
        TextLabel lblScore;
        TextLabel lblSCORE;
        TextLabel lblTIMER;
        TextLabel lbltimer;
        GameObject food;
        GameObject TopBorder;
        GameObject BottomBorder;
        GameObject RightBorder;
        GameObject LeftBorder;
        GameObject SuperFood;
        GameObject BadFood;
        public static int bodyLength = 1;


        //This function is called by the game one time on initialization!
        //Here you should define game board resolution and size (x,y).
        //Here you should initialize all variables defined above and create all visual objects on screen.
        //You could also start game background music here.
        //use board Object to add game objects to the game board, play background music, set interval, etc...
        public void GameInit(Board board)
        {

            //Setup board size and resolution!
            Board.resolutionFactor = 2;
            board.XSize = 420;
            board.YSize = 400;
            //-----Finished Board----

            //Adding a the scores label to the game board.
            Position labelPosition = new Position(0, 25);
            lblScore = new TextLabel("SCORE:", labelPosition);
            lblScore.SetFont("comics sans", 10);
            board.AddLabel(lblScore);

            Position scorePosition = new Position(8, 25);
            lblScore = new TextLabel(bodyLength.ToString(), scorePosition);
            lblScore.SetFont("comics sans", 10);
            board.AddLabel(lblScore);

            //adding the timer
            Position TIMERPosition = new Position(180,550);
            lblScore = new TextLabel("Time:", labelPosition);
            lblScore.SetFont("comics sans", 12);
            board.AddLabel(lblScore);

            Position timePosition = new Position(200, 550);
            lblScore = new TextLabel(bodyLength.ToString(), scorePosition);
            lblScore.SetFont("comics sans", 12);
            board.AddLabel(lblScore);
            //----- Finished Labels-----



            //Adding Game Object
            //Position foodPosition = new Position(200, 100);
            //food = new GameObject(foodPosition, 20, 20);
            //food.SetImage(Properties.Resources.food);
            //food.direction = GameObject.Direction.RIGHT;
            //board.AddGameObject(food);
            //setting the board
            board.SetBackgroundImage(Properties.Resources.merets);
            

            //adding the snake movement
            snakeBody = new GameObject[bodyLength];
            Position snakeHeadPosition = new Position(100, 50);
            snakeBody[0] = new GameObject(snakeHeadPosition, 10, 10);
            snakeBody[0].SetImage(Properties.Resources.yodasigma);
            snakeBody[0].direction = GameObject.Direction.RIGHT;
            board.AddGameObject(snakeBody[0]);

            // ---------------


            //borders
            Position TopBorderPosition = new Position(0, 0);
            TopBorder = new GameObject(TopBorderPosition,400, 20);
            TopBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(TopBorder);

            Position BottomBorderPosition = new Position(380, 0);
            BottomBorder = new GameObject(BottomBorderPosition, 400, 20);
            BottomBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(BottomBorder);

            Position RightBorderPosition = new Position(0, 380);
            RightBorder = new GameObject(RightBorderPosition, 20,400);
            RightBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(RightBorder);

            Position LeftBorderPosition = new Position(0, 0);
            LeftBorder = new GameObject(LeftBorderPosition, 20, 400);
            LeftBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(LeftBorder);



            //Play file in loop!

            board.PlayBackgroundMusic(@"\Images\Dynoro & Gigi DAgostino - In My Mind.mp3");


            //Play file once!
            //board.PlayShortMusic(@"\Images\eat.wav");


            //Start game timer!
            board.StartTimer(50);
        }
        
        
        //This function is called frequently based on the game board interval that was set when starting the timer!
        //Use this function to move game objects and check collisions
        public void GameClock(Board board)
        {
            Position snakeHeadPosition = snakeBody[0].GetPosition();
            if (snakeBody[0].direction == GameObject.Direction.RIGHT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y + 5;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.LEFT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y - 5;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.UP)
            {
                snakeHeadPosition.X = snakeHeadPosition.X - 5;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.DOWN)
            {
                snakeHeadPosition.X = snakeHeadPosition.X + 5;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }

        }

        //This function is called by the game when the user press a key down on the keyboard.
        //Use this function to check the key that was pressed and change the direction of game objects acordingly.
        //Arrows ascii codes are given by ConsoleKey.LeftArrow and alike
        //Also use this function to handle game pause, showing user messages (like victory) and so on...
        public void KeyDown(Board board, char key)
        {
            if (key == (char)ConsoleKey.LeftArrow)
                snakeBody[0].direction = GameObject.Direction.LEFT;
            if (key == (char)ConsoleKey.RightArrow)
                snakeBody[0].direction = GameObject.Direction.RIGHT;
            if (key == (char)ConsoleKey.UpArrow)
                snakeBody[0].direction = GameObject.Direction.UP;
            if (key == (char)ConsoleKey.DownArrow)
                snakeBody[0].direction = GameObject.Direction.DOWN;

        }
    }
}
