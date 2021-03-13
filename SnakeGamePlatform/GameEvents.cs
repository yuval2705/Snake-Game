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
        //declering all the verrabls
        public static int timerDifficulty = 120;
        GameObject[] snakeBody;
        TextLabel lblScore;
        TextLabel lbltimer;
        GameObject TopBorder;
        GameObject BottomBorder;
        GameObject RightBorder;
        GameObject LeftBorder;
        public static int bodyLength = 1;
        GameObject Food;
        public static int FoodCount = 0;

        GameObject SuperFood;
        public static int SuperFoodCount = 0;

        GameObject BadFood;
        public static int BadFoodCount = 0;
        
        //placing the game borders, must have them or the game will be unloseble
        public void PlaceBorders(Board board)
        {
            Position TopBorderPosition = new Position(0, 0);
            TopBorder = new GameObject(TopBorderPosition, 400, 20);
            TopBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(TopBorder);

            Position BottomBorderPosition = new Position(380, 0);
            BottomBorder = new GameObject(BottomBorderPosition, 400, 20);
            BottomBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(BottomBorder);

            Position RightBorderPosition = new Position(0, 380);
            RightBorder = new GameObject(RightBorderPosition, 20, 400);
            RightBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(RightBorder);

            Position LeftBorderPosition = new Position(0, 0);
            LeftBorder = new GameObject(LeftBorderPosition, 20, 400);
            LeftBorder.SetImage(Properties.Resources.Black);
            board.AddGameObject(LeftBorder);
        }

        //placing the score for the player to see
        public void PlaceScore(Board board)
        {
            Position labelPosition = new Position(0, 25);
            lblScore = new TextLabel($"SCORE:{(bodyLength-1).ToString()}", labelPosition);
            lblScore.SetFont("comics sans", 10);
            board.AddLabel(lblScore);
        }

        //placing the timer fot the player
        public void PlaceTimer(Board board)
        {
            Position timerPosition = new Position(180, 550);
            lbltimer = new TextLabel("Time:", timerPosition);
            lbltimer.SetFont("comics sans", 12);
            board.AddLabel(lbltimer);
        }

        //a function that help organize the project by getting all the first declares of objects for the game to work
        public void PlaceFirstGameObjects(Board board)
        {
            //snake
            //CreatSnakeByLength(board,0);


            //food
            Position FoodPosition = new Position(0,0);
            Food = new GameObject(FoodPosition, 10, 10);
            Food.SetImage(Properties.Resources.yona);

            //super food
            Position SuperFoodPosition = new Position(0,0);
            SuperFood = new GameObject(SuperFoodPosition, 10, 10);
            SuperFood.SetImage(Properties.Resources.food);

            //bad food
            Position BadFoodPosition = new Position(0,0);
            BadFood = new GameObject(BadFoodPosition, 10, 10);
            BadFood.SetImage(Properties.Resources.food);
        }
        public void GameInit(Board board)
        {

            //Setup board size and resolution!
            Board.resolutionFactor = 2;
            board.XSize = 420;
            board.YSize = 400;
            board.SetBackgroundImage(Properties.Resources.merets);
            //-----Finished Board----


            //Score
            PlaceScore(board);
            //

            //Timer
            PlaceTimer(board);
            //

            //----- Finished Labels-----


            //creating the game objects
            PlaceFirstGameObjects(board);
            snakeBody = new GameObject[bodyLength];

            //snake head
            Position snakeHeadPosition = new Position(100, 100);
            snakeBody[0] = new GameObject(snakeHeadPosition, 10, 10);
            snakeBody[0].SetImage(Properties.Resources.yodasigma);
            snakeBody[0].direction = GameObject.Direction.RIGHT;
            board.AddGameObject(snakeBody[0]);
            // ---------------


            //borders
            PlaceBorders(board);
            //


            //background music
            //board.PlayBackgroundMusic(@"\Images\Dynoro & Gigi DAgostino - In My Mind.mp3");
            //

            //Play file once!
            //board.PlayShortMusic(@"\Images\eat.wav");


            //Start game timer!
            board.StartTimer(timerDifficulty);

        }
        
        
        //This function is called frequently based on the game board interval that was set when starting the timer!
        //Use this function to move game objects and check collisions
        public void GameClock(Board board)
        {
            
            GameOver(board);
            lblScore.SetText($"SCORE:{bodyLength-1}");
            PlaceTimer(board);

            SnakeMovement();
            Position snakeHeadPosition = snakeBody[0].GetPosition();
            if (snakeBody[0].direction == GameObject.Direction.RIGHT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y + 10;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.LEFT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y - 10;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.UP)
            {
                snakeHeadPosition.X = snakeHeadPosition.X - 10;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.DOWN)
            {
                snakeHeadPosition.X = snakeHeadPosition.X + 10;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            
            SnakeEatenFood(board);
            AddFood(board);

        }

        public void KeyDown(Board board, char key)
        {
            if (key == (char)ConsoleKey.LeftArrow && snakeBody[0].direction != GameObject.Direction.RIGHT)
            {
                snakeBody[0].direction = GameObject.Direction.LEFT;
                snakeBody[0].SetImage(Properties.Resources.yodasigmaLEFT);
            } 
            if (key == (char)ConsoleKey.RightArrow && snakeBody[0].direction != GameObject.Direction.LEFT)
            {
                snakeBody[0].direction = GameObject.Direction.RIGHT;
                snakeBody[0].SetImage(Properties.Resources.yodasigmaRIGHT);
            }  
            if (key == (char)ConsoleKey.UpArrow && snakeBody[0].direction != GameObject.Direction.DOWN)
            {
                snakeBody[0].direction = GameObject.Direction.UP;
                snakeBody[0].SetImage(Properties.Resources.yodasigma);
            }
            if (key == (char)ConsoleKey.DownArrow && snakeBody[0].direction != GameObject.Direction.UP)
            {
                snakeBody[0].direction = GameObject.Direction.DOWN;
                snakeBody[0].SetImage(Properties.Resources.yodasigma);
                snakeBody[0].SetImage(Properties.Resources.yodasigmaDOWN);
            }
        }

        //checks if the player has lost and "uploads" the "GAME OVER" screen
        public void GameOver(Board board)
        {
            bool isOver = (OBJInteractsWithSnake(board,snakeBody[0])||snakeBody[0].IntersectWith(LeftBorder) || snakeBody[0].IntersectWith(RightBorder) || snakeBody[0].IntersectWith(BottomBorder) || snakeBody[0].IntersectWith(TopBorder));
            if (isOver)
            {
                board.StopBackgroundMusic();

                board.PlayShortMusic(@"\Images\loser.wav");


                Position labelPosition = new Position(120, 100);
                lblScore = new TextLabel($"SCORE:{(bodyLength-1).ToString()}", labelPosition);
                lblScore.SetFont("comics sans", 50);

                Position gameOverPosition = new Position(70,100);
                TextLabel GAMEOVER = new TextLabel("GAME OVER",gameOverPosition);
                GAMEOVER.SetFont("comics sans", 60);

                board.AddLabel(GAMEOVER);
                board.AddLabel(lblScore);
                board.StopTimer();

            }
            // add a reset funcation for all te values
        }

        /// <summary>
        /// generate the "Food" item type on the board
        /// </summary>
        /// <param name="board"></param>
        public void AddFood(Board board)
        {
            if (FoodCount < 1)
            {
                Random foodX = new Random();
                Random foodY = new Random();

                Food.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                bool IsInteractsWithSomething = Food.OnScreen(board) && !Food.IntersectWith(snakeBody[0]) && !Food.IntersectWith(SuperFood) && !Food.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, Food) && !Food.IntersectWith(LeftBorder) && !Food.IntersectWith(RightBorder) && !Food.IntersectWith(BottomBorder) && !Food.IntersectWith(TopBorder);
                while(!IsInteractsWithSomething)
                {
                    Food.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                    IsInteractsWithSomething = Food.OnScreen(board) && !Food.IntersectWith(snakeBody[0]) && !Food.IntersectWith(SuperFood) && !Food.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, Food) && !Food.IntersectWith(LeftBorder) && !Food.IntersectWith(RightBorder) && !Food.IntersectWith(BottomBorder) && !Food.IntersectWith(TopBorder);
                }
                FoodCount++;
                board.AddGameObject(Food);
            }
        }

        //generate the "SuperFood" item type on the board
        public void AddSuperFood(Board board)
        {
            if (SuperFoodCount < 1)
            {
                Random foodX = new Random();
                Random foodY = new Random();

                SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                bool IsInteractsWithSomething = !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
                while (!IsInteractsWithSomething)
                {
                    SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                    IsInteractsWithSomething = !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
                }
                SuperFoodCount++;
                board.AddGameObject(SuperFood);
            }
        }

        //generate the "BadFood" item type on the board
        public void AddBadFood(Board board)
        {
            if (BadFoodCount < 1)
            {
                Random foodX = new Random();
                Random foodY = new Random();

                BadFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                bool IsInteractsWithSomething = !BadFood.IntersectWith(snakeBody[0]) && !BadFood.IntersectWith(SuperFood) && !BadFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, BadFood) && !BadFood.IntersectWith(LeftBorder) && !BadFood.IntersectWith(RightBorder) && !BadFood.IntersectWith(BottomBorder) && !BadFood.IntersectWith(TopBorder);
                while (!IsInteractsWithSomething)
                {
                    BadFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                    IsInteractsWithSomething = !BadFood.IntersectWith(snakeBody[0]) && !BadFood.IntersectWith(SuperFood) && !BadFood.IntersectWith(Food) && !OBJInteractsWithSnake(board, BadFood) && !BadFood.IntersectWith(LeftBorder) && !BadFood.IntersectWith(RightBorder) && !BadFood.IntersectWith(BottomBorder) && !BadFood.IntersectWith(TopBorder);
                }
                BadFoodCount++;
                board.AddGameObject(BadFood);
            }
        }

        //check if a specific object is interacting with the snake body (only his body)
        public bool OBJInteractsWithSnake(Board board,GameObject obj)
        {
            for (int i = 1; i < bodyLength; i++)
            {
                if (obj.IntersectWith(snakeBody[i]))
                    return true;
            }
            return false;
        }

        //places the whole body of the snake on the board 
        public void PlaceWholeSnake(Board board)
        {
            for (int i = 0; i < bodyLength; i++)
            {
                board.AddGameObject(snakeBody[i]);
            }
        }

        //a function that copy the snake's body and increases it (the array not snake it self) length by one
        public static GameObject[] CopyAndIncreaseArrayByOne(GameObject[] arr)
        {
            bodyLength++;
            GameObject[] copy = new GameObject[bodyLength];
            for (int i = 0; i < bodyLength - 1; i++)
            {
                copy[i] = arr[i];
            }
            return copy;
        }

        //a function that increases the snake's length by one
        public void SnakeGrowByOne(Board board)
        {
            bodyLength++;
            GameObject[] copy = new GameObject[bodyLength];
            for (int i = 0; i < bodyLength - 1; i++)
            {
                copy[i] = snakeBody[i];
            }
            Position newBodyPosition = copy[bodyLength - 2].GetPosition();
            copy[bodyLength - 1] = new GameObject(newBodyPosition, 10, 10);
            copy[bodyLength - 1].SetImage(Properties.Resources.food);

            if (snakeBody[bodyLength - 2].direction == GameObject.Direction.RIGHT)
            {
                newBodyPosition.Y = newBodyPosition.Y - 10;
                copy[bodyLength - 1].SetPosition(newBodyPosition);
            }
            else if (snakeBody[bodyLength - 2].direction == GameObject.Direction.LEFT)
            {
                newBodyPosition.Y = newBodyPosition.Y + 10;
                copy[bodyLength - 1].SetPosition(newBodyPosition);
            }
            else if (snakeBody[bodyLength - 2].direction == GameObject.Direction.UP)
            {
                newBodyPosition.X = newBodyPosition.X + 10;
                copy[bodyLength - 1].SetPosition(newBodyPosition);
            }
            else if (snakeBody[bodyLength-2].direction == GameObject.Direction.DOWN)
            {
                newBodyPosition.X = newBodyPosition.X - 10;
                copy[bodyLength - 1].SetPosition(newBodyPosition);
            }
            board.AddGameObject(copy[bodyLength - 1]);
            copy[bodyLength - 1].SetImage(Properties.Resources.food);
            snakeBody = copy;
            snakeBody[bodyLength - 1].direction = snakeBody[bodyLength - 2].direction;
            board.AddGameObject(snakeBody[bodyLength-1]);
            timerDifficulty -= 2;
            board.StartTimer(timerDifficulty);


        }

        //a function that allow as to increase the snake's length by a specific number
        public void SnakeGrowByNumber(Board board,int GrowBy)
        {
            for (int i = 0; i < GrowBy; i++)
            {
                SnakeGrowByOne(board);
            }
        }

        //a function that allow as to create a snake by a specific length
        public void CreatSnakeByLength(Board board,int length)
        {
            snakeBody = new GameObject[bodyLength];

            //snake head
            Position snakeHeadPosition = new Position(100, 100);
            snakeBody[0] = new GameObject(snakeHeadPosition, 10, 10);
            snakeBody[0].SetImage(Properties.Resources.yodasigma);
            snakeBody[0].direction = GameObject.Direction.RIGHT;

            //snake body
            SnakeGrowByNumber(board,length - 1);

            PlaceWholeSnake(board);
        }

        //Making the snake move in the right order(one peace always coming to the place of the peace that is infront of her)
        public void SnakeMovement()
        {
            for (int i = bodyLength-1; i > 0; i--)
            {
                snakeBody[i].SetPosition(snakeBody[i - 1].GetPosition());
            }
        }

        //checking if the snake has eaten a type of food 
        public void SnakeEatenFood(Board board)
        {
            if (snakeBody[0].IntersectWith(Food))
            {
                //board.PauseBackgroundMusic();
                board.PlayShortMusic(@"\Images\food.wav");
                SnakeGrowByOne(board);
                FoodCount--;
                //board.PlayBackgroundMusic(@"\Images\Dynoro & Gigi DAgostino - In My Mind.mp3");
            }
            if (snakeBody[0].IntersectWith(SuperFood))
            {
                board.PlayShortMusic(@"\Images\superfood.wav");
                //board.PlayShortMusic(@"\Images\איציקyonimisthebest.wav");
                SnakeGrowByNumber(board, 3);
                AddSuperFood(board);
                
            }
            if (snakeBody[0].IntersectWith(BadFood))
            {
                //board.PlayShortMusic(@"\Images\loser.wav");
                //snake loses one score
                AddBadFood(board);
            }
        }
    }
}
