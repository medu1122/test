using System;
namespace conran2
{
    class SnakeGame
    {
        #region
        public Random rand = new Random();
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        int score, headX, headY, fruitX, fruitY, nTail;
        int[] tailX = new int[100];
        int[] tailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool gameOver, reset, isprinted, horizontal, vertical;
        string dir, pre_dir;
        #endregion
        void ShowBanner()
        {
            Console.SetWindowSize(width, height + panel);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("GAME ");
            Console.WriteLine("Press any key to play");
            Console.WriteLine("Press e key to PAUSE ");
            Console.WriteLine("Press q key to quit");
            Console.WriteLine("press r key to reset");
            // key 
            ConsoleKeyInfo keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
            //else if (keypress.Key == ConsoleKey.P)
        }

        void Setup() //huong di cho ran
        {
            dir = "RIGHT"; pre_dir = "";
            score = 0; nTail = 0;
            gameOver = reset = isprinted = false;
            //spawn location
            headX = 2;
            headY = 2;
            // gift
            RandomFruit();

        }
        private void RandomFruit()
        {
            int typefruit = rand.Next(1,3);
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);

        }
        void Update()
        {
            while (!gameOver)
            {
                CheckInput();
                Logic();
                Render();
                if (reset) { break; }
                //dung man hinh 
                Thread.Sleep(150);

            }
            if (gameOver) { Lose(); }
        }


        //KEY DIEU KHIEN
        void CheckInput() //key
        {
            while (Console.KeyAvailable)
            {   //key bat ky
                keypress = Console.ReadKey(true);
                pre_dir = dir; // luu lai huong di truoc do
                switch (keypress.Key)
                {
                    case ConsoleKey.Q: Environment.Exit(0); break;
                    case ConsoleKey.E: dir = "STOP"; break;
                    case ConsoleKey.W: dir = "UP"; break;
                    case ConsoleKey.A: dir = "LEFT"; break;
                    case ConsoleKey.S: dir = "DOWN"; break;
                    case ConsoleKey.D: dir = "RIGHT"; break;
                }
            }

        }


        // HIEN THI DOI TUONG
        void Logic()
        {
            int preX = tailX[0], prey = tailY[0];
            int tempx, tempy;
            if (dir != "STOP")
            {
                /* tailX[0] = headX; tailY[0] = headY;
                List<int> lst = new List<int>();
                lst.Insert */
                for (int i = 1; i < nTail; i++)
                {
                    tempx = tailX[i]; tempy = tailY[i];
                    tailX[i] = preX;
                    tailY[i] = prey;
                    preX = tempx;
                    prey = tempy;

                }
            }
            switch (dir)
            {
                case "RIGHT": headX++; break;
                case "LEFT": headX--; break;
                case "UP": headY--; break;
                case "DOWN": headY++; break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("STOP");
                            Console.WriteLine("GAME ");
                            Console.WriteLine("Press any key to play");
                            Console.WriteLine("Press e key to PAUSE ");
                            Console.WriteLine("Press q key to quit");
                            // key 
                            ConsoleKeyInfo keypress = Console.ReadKey(true);
                            if (keypress.Key == ConsoleKey.Q)
                            {
                                Environment.Exit(0);
                            }
                            if (keypress.Key == ConsoleKey.R) { reset = true; break; }
                            if (keypress.Key == ConsoleKey.P)
                            {
                                break;
                            }
                        }
                        dir = pre_dir; break;
                    }
                    break;
            }

        }


        void Render()
        {

        }


        // DIE 
        void Lose() { }



        //MAIN 
        static void Main(string[] args)
        {
            //note
            /*  Console.SetCursorPosition(0, 10);
              Console.BackgroundColor = ConsoleColor.Green;
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Write("HELO");
              ConsoleKeyInfo keyinfo = Console.ReadKey();
              switch (keyinfo.Key)
              {
                  case ConsoleKey.W: Console.ReadKey(); break;
                  case ConsoleKey.S: Console.ReadKey();break;
                  case ConsoleKey.A: Console.ReadKey(); break;
                  case ConsoleKey.D: Console.ReadKey(); break;

              } */

            SnakeGame snakegame = new SnakeGame();
            snakegame.ShowBanner();
            while (true)
            {
                //clear console ... 
                Console.Clear();
            }
            Console.ReadKey();
        }
    }
}