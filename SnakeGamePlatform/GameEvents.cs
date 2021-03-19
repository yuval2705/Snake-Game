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
    //To Do
    // finish the bad food spawning algorithem - yuval
    // finish the game over message - yuval

    public class GameEvents:IGameEvents
    {
        //declering all the verrabls
        public static int timerDifficulty = 140;
        GameObject[] snakeBody;
        TextLabel lblScore;
        TextLabel lbltimer;
        TextLabel lblPressSpace;
        GameObject TopBorder;
        GameObject BottomBorder;
        GameObject RightBorder;
        GameObject LeftBorder;
        GameObject StartMenu;
        public int timer = 0;
        public int MemberOfKnesset = 1;

        public static int BodySize = 20;

        public static int bodyLength = 1;
        GameObject Food;
        public static bool Foodstatus = false;

        GameObject SuperFood;
        public static bool SuperFoodstatus = false;
        public static int SuperFoodTimer = 0;
        public static int TimeToAppear = 5;
        public static int TimeToDisappear = 5;

        GameObject BadFood;
        public static bool BadFoodstatus = false;
        public static int BadFoodTimer = 0;

        public static bool gamewasover = false;
        TextLabel GAMEOVER;
        TextLabel FunnyEnding;


        //placing the game borders, must have them or the game will be unloseble
        public void PlaceBorders(Board board)
        {
            Position TopBorderPosition = new Position(0, 0);
            TopBorder = new GameObject(TopBorderPosition, 400, 20);
            TopBorder.SetBackgroundColor(Color.White);
            board.AddGameObject(TopBorder);

            Position BottomBorderPosition = new Position(380, 0);
            BottomBorder = new GameObject(BottomBorderPosition, 400, 20);
            BottomBorder.SetBackgroundColor(Color.White);
            board.AddGameObject(BottomBorder);

            Position RightBorderPosition = new Position(0, 400);
            RightBorder = new GameObject(RightBorderPosition, 20, 400);
            RightBorder.SetBackgroundColor(Color.White);
            board.AddGameObject(RightBorder);

            Position LeftBorderPosition = new Position(0, 0);
            LeftBorder = new GameObject(LeftBorderPosition, 20, 400);
            LeftBorder.SetBackgroundColor(Color.White);
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
            Food = new GameObject(FoodPosition, BodySize, BodySize);
            Food.SetImage(Properties.Resources.yona);

            //super food
            Position SuperFoodPosition = new Position(0,0);
            SuperFood = new GameObject(SuperFoodPosition, BodySize, BodySize);
            SuperFood.SetImage(Properties.Resources.petegmeretz);

            //bad food
            Position BadFoodPosition = new Position(0,0);
            BadFood = new GameObject(BadFoodPosition, BodySize, BodySize);
            BadFood.SetImage(Properties.Resources.licod);
        }
        public void GameInit(Board board)
        {

            //Setup board size and resolution!
            Board.resolutionFactor = 2;
            board.XSize = 420;
            board.YSize = 420;
            //board.SetBackgroundImage(Properties.Resources.merets);
            board.SetBackgroundColor(Color.Green);
            //-----Finished Board----

            if (!gamewasover)
            {
                StartMenu = new GameObject(new Position(0, 0), 420, 400);
                StartMenu.SetImage(Properties.Resources.rozen_menu);
                board.AddGameObject(StartMenu);
            }
            else
            {
                Position pressPosition = new Position(70, 100);
                lblPressSpace = new TextLabel("Press space to start again!", pressPosition);
                lblPressSpace.SetFont(FontStyle.Italic.ToString(), 40);
                board.AddLabel(lblPressSpace);
            }


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
            snakeBody[0] = new GameObject(snakeHeadPosition, BodySize, BodySize);
            snakeBody[0].SetImage(Properties.Resources.yodasigmaRIGHT);
            snakeBody[0].direction = GameObject.Direction.RIGHT;
            board.AddGameObject(snakeBody[0]);
            // ---------------


            //borders
            PlaceBorders(board);
            //


            //background music
            //board.PlayBackgroundMusic(@"\Images\Dynoro & Gigi DAgostino - In My Mind.mp3");

            //Start game timer!
        }
 
        //This function is called frequently based on the game board interval that was set when starting the timer!
        //Use this function to move game objects and check collisions
        public void GameClock(Board board)
        {
            timer += timerDifficulty;
            SnakeMovement();

            lblScore.SetText($"SCORE:{bodyLength-1}");
            lbltimer.SetText($"timer:{timer/1000}");
            
            Position snakeHeadPosition = snakeBody[0].GetPosition();
            if (snakeBody[0].direction == GameObject.Direction.RIGHT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y + BodySize;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.LEFT)
            {
                snakeHeadPosition.Y = snakeHeadPosition.Y - BodySize;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.UP)
            {
                snakeHeadPosition.X = snakeHeadPosition.X - BodySize;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            else if (snakeBody[0].direction == GameObject.Direction.DOWN)
            {
                snakeHeadPosition.X = snakeHeadPosition.X + BodySize;
                snakeBody[0].SetPosition(snakeHeadPosition);
            }
            GameOver(board);
            SnakeEatenFood(board);
            AddFood(board);
            AddSuperFood(board);

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
            if (key == (char)ConsoleKey.Spacebar)
            {
                if (gamewasover)
                {
                    ParametersReset(board);
                    GameInit(board);
                    board.RemoveGameText(lblPressSpace);
                    gamewasover = false;
                }
                board.StartTimer(timerDifficulty);
                board.RemoveGameObject(StartMenu);
            }
        }

        //checks if the player has lost and "uploads" the "GAME OVER" screen
        public void GameOver(Board board)
        {
            bool isOver = (OBJInteractsWithSnake(board,snakeBody[0])||snakeBody[0].IntersectWith(LeftBorder) || snakeBody[0].IntersectWith(RightBorder) || snakeBody[0].IntersectWith(BottomBorder) || snakeBody[0].IntersectWith(TopBorder));
            if (isOver)
            {
                board.StopBackgroundMusic();
                board.PlayShortMusic(@"\Images\endgame.wav");

                board.RemoveGameText(lblScore);
                Position labelPosition = new Position(120, 100);
                lblScore = new TextLabel($"SCORE:{(bodyLength-1).ToString()}", labelPosition);
                lblScore.SetFont("comics sans", 50);

                board.RemoveGameText(lbltimer);


                Position gameOverPosition = new Position(70,100);
                GAMEOVER = new TextLabel("GAME OVER",gameOverPosition);
                GAMEOVER.SetFont("comics sans", 60);

                
                board.AddLabel(GAMEOVER);
                board.AddLabel(lblScore);

                Random IndexOfEnding = new Random();
                int ezer = IndexOfEnding.Next(0, 5);
                string funnySetence = "";
                switch (ezer)
                {
                    case 0:
                        funnySetence = "עוד קצת והיינו עוברים את אחוז החסימה";
                        break;
                    case 1:
                        funnySetence = "שלטון ימין זכה";
                        break;
                    case 2:
                        funnySetence = "אין כלום כי לא היה כלום";
                        break;
                    case 3:
                        funnySetence = "יש!!! עברנו";
                        break;
                    case 4:
                        funnySetence = "מעניין איזה תיק נקבל...";
                        break;
                }
                Position FunnySetencePosition = new Position(180, 50);
                FunnyEnding = new TextLabel(funnySetence, FunnySetencePosition);
                FunnyEnding.SetFont("comics sans", 40);
                board.AddLabel(FunnyEnding);

                board.StopTimer();

                //getting rid of the game objects that are still in the screen
                for (int i = 0; i < bodyLength; i++)
                {
                    board.RemoveGameObject(snakeBody[i]);
                }
                board.RemoveGameObject(Food);
                board.RemoveGameObject(SuperFood);
                board.RemoveGameObject(BadFood);
                board.SetBackgroundColor(Color.Red);
                //letting the user to restart the game by pressing "SPACE"

                gamewasover = true;
            }
        }

        // add a reset funcation for all te values
        public void ParametersReset(Board board)
        {
            //removing all the game objects
            for (int i = 0; i < bodyLength; i++)
            {
                board.RemoveGameObject(snakeBody[i]);
            }
            board.RemoveGameObject(Food);
            board.RemoveGameObject(SuperFood);
            board.RemoveGameObject(BadFood);
            board.RemoveGameObject(LeftBorder);
            board.RemoveGameObject(RightBorder);
            board.RemoveGameObject(TopBorder);
            board.RemoveGameObject(BottomBorder);


            // reseting all the game labels
            board.RemoveGameText(GAMEOVER);
            board.RemoveGameText(lblScore);
            board.RemoveGameText(lbltimer);
            board.RemoveGameText(FunnyEnding);


            // reseting all the helping parameters
            timerDifficulty = 120;
            bodyLength = 1;
            Foodstatus = false;
            SuperFoodstatus = false;
            BadFoodstatus = false;
            MemberOfKnesset = 1;
            timer = 0;
            

        }

        /// <summary>
        /// generate the "Food" item type on the board
        /// </summary>
        /// <param name="board"></param>
        public void AddFood(Board board)
        {
            if (!Foodstatus)
            {
                Random foodX = new Random();
                Random foodY = new Random();

                Food.SetPosition(new Position(foodX.Next(0, 421), Math.Abs(foodY.Next(0, 800) - 421 - foodX.Next(0,200))));
                bool IsInteractsWithSomething = Food.OnScreen(board) && !Food.IntersectWith(snakeBody[0]) && !Food.IntersectWith(SuperFood) && !Food.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, Food) && !Food.IntersectWith(LeftBorder) && !Food.IntersectWith(RightBorder) && !Food.IntersectWith(BottomBorder) && !Food.IntersectWith(TopBorder);
                while(!IsInteractsWithSomething)
                {
                    Food.SetPosition(new Position(foodX.Next(0, 421), Math.Abs(foodY.Next(0, 800) - 421 - foodX.Next(0, 200))));
                    IsInteractsWithSomething = Food.OnScreen(board) && !Food.IntersectWith(snakeBody[0]) && !Food.IntersectWith(SuperFood) && !Food.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, Food) && !Food.IntersectWith(LeftBorder) && !Food.IntersectWith(RightBorder) && !Food.IntersectWith(BottomBorder) && !Food.IntersectWith(TopBorder);
                }
                Foodstatus = true;
                board.AddGameObject(Food);
            }
        }

        //generate the "SuperFood" item type on the board
        public void AddSuperFood(Board board)
        {
            SuperFoodTimer += timerDifficulty;

            if (SuperFoodTimer >= TimeToAppear*1000 && !SuperFoodstatus)
            {
                Random foodX = new Random();
                Random foodY = new Random();

                SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                bool IsInteractsWithSomething = Food.OnScreen(board) && !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
                while (!IsInteractsWithSomething)
                {
                    SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
                    IsInteractsWithSomething = Food.OnScreen(board) && !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
                }
                board.AddGameObject(SuperFood);
                SuperFoodTimer = 0;
                SuperFoodstatus = true;
            }
            else if(SuperFoodTimer >= TimeToDisappear * 1000 && SuperFoodstatus)
            {
                SuperFood.SetPosition(new Position(0, 0));
                SuperFoodstatus = false;
                SuperFoodTimer = 0;
            }

            //if (SuperFoodstatus && (timer/1000)%12 == 0)
            //{
            //    SuperFood.SetPosition(new Position(0,0));
            //    SuperFoodstatus = false;
            //}
            //else if(!SuperFoodstatus && (timer / 1000) % 12 == 0)
            //{
            //    Random foodX = new Random();
            //    Random foodY = new Random();

            //    SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
            //    bool IsInteractsWithSomething = Food.OnScreen(board) && !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
            //    while (!IsInteractsWithSomething)
            //    {
            //        SuperFood.SetPosition(new Position(foodX.Next(20, 421), foodY.Next(20, 381)));
            //        IsInteractsWithSomething = Food.OnScreen(board) && !SuperFood.IntersectWith(snakeBody[0]) && !SuperFood.IntersectWith(Food) && !SuperFood.IntersectWith(BadFood) && !OBJInteractsWithSnake(board, SuperFood) && !SuperFood.IntersectWith(LeftBorder) && !SuperFood.IntersectWith(RightBorder) && !SuperFood.IntersectWith(BottomBorder) && !SuperFood.IntersectWith(TopBorder);
            //    }
            //    board.AddGameObject(SuperFood);
            //    SuperFoodstatus = true;
            //}
        }

        //generate the "BadFood" item type on the board
        public void AddBadFood(Board board)
        {
            if (BadFoodstatus)
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
                BadFoodstatus = false;
                board.AddGameObject(BadFood);
            }
        }

        //check if a specific object is interacting with the snake body (only his body)
        public bool OBJInteractsWithSnake(Board board,GameObject obj)
        {
            for (int i = 2; i < bodyLength; i++)
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
            copy[bodyLength - 1] = new GameObject(newBodyPosition, BodySize, BodySize);

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
            copy[bodyLength - 1].SetImage(Properties.Resources.cos);
            switch (MemberOfKnesset)
            {
                case 1:
                    copy[bodyLength - 1].SetImage(Properties.Resources.nitzanhorvits);
                    MemberOfKnesset++;
                    break;
                case 2:
                    copy[bodyLength - 1].SetImage(Properties.Resources.tamarzanberg);
                    MemberOfKnesset++;
                    break;
                case 3:
                    copy[bodyLength - 1].SetImage(Properties.Resources.yairgolan);
                    MemberOfKnesset++;
                    break;
                case 4:
                    copy[bodyLength - 1].SetImage(Properties.Resources.zoabi);
                    MemberOfKnesset++;
                    break;
                case 5:
                    copy[bodyLength - 1].SetImage(Properties.Resources.fridge);
                    MemberOfKnesset = 1;
                    break;
            }
            snakeBody = copy;
            snakeBody[bodyLength - 1].direction = snakeBody[bodyLength - 2].direction;
            board.AddGameObject(snakeBody[bodyLength - 1]);
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
            snakeBody[0] = new GameObject(snakeHeadPosition, BodySize, BodySize);
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
                board.PauseBackgroundMusic();
                board.PlayShortMusic(@"\Images\eatFood.wav");
                SnakeGrowByOne(board);
                Foodstatus = false;
                board.ResumeBackgroundMusic();
            }
            if (snakeBody[0].IntersectWith(SuperFood))
            {
                board.PauseBackgroundMusic();
                board.PlayShortMusic(@"\Images\eatSuperFood.wav");
                SnakeGrowByNumber(board, 2);
                AddSuperFood(board);
                board.ResumeBackgroundMusic();
                SuperFood.SetPosition(new Position(0, 0));
                SuperFoodstatus = false;
                SuperFoodTimer = 0;
                

            }
            if (snakeBody[0].IntersectWith(BadFood))
            {
                board.PauseBackgroundMusic();
                board.PlayShortMusic(@"\Images\gameover.wav");
                AddBadFood(board);
                board.ResumeBackgroundMusic();
                BadFood.SetPosition(new Position(0, 0));
                
            }
        }

    }
}
