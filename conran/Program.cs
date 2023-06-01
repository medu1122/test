
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection.Metadata;

namespace CGO_ThucHanh1
{
    class SnakeGameNe
    {
        #region parameter
        public Random rand = new Random();
        public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
        int score, headX, headY, fruitX, fruitY, nTail, score_Max = 0;
        int[] TailX = new int[100];
        int[] TailY = new int[100];
        const int height = 20;
        const int width = 60;
        const int panel = 10;
        bool gameOver, reset, isprinted, horizontal, vertical;
        string dir, pre_dir;
        #endregion
        //Hiển thị màn hình khi bắt đầu
        void ShowBanner()
        {
            Console.SetWindowSize(width, height + panel);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false; //tắt con trỏ nháy nháy
            Console.WriteLine("===SNAKE GAME===");
            Console.WriteLine("Press any key to play");
            Console.WriteLine("Tips: - Press P key to PAUSE game");
            Console.WriteLine("      - Press R key to RESET game");
            Console.WriteLine("      - Press Q key to QUIT game");

            keypress = Console.ReadKey(true);
            if (keypress.Key == ConsoleKey.Q)
                Environment.Exit(0);
        }

        //Cài đặt thông số ban đầu
        void Setup()
        {
            dir = "RIGHT"; pre_dir = "";    //bước đi đầu tiên qua phải 
            score = 0; nTail = 0;
            gameOver = reset = isprinted = false;
            headX = 20; //ko vuot qua width (vi tri bat dau Ran)
            headY = 10; //ko vuot qua height(vi tri bat dau Ran)
            randonQua();
        }
        
        //MAX SCORE 

        public int Max_Score ()
        {
            string take_max_score1 = "score.txt";
            string take_max_score2 = File.ReadAllText(take_max_score1);
            if (int.TryParse(take_max_score2, out score_Max))
            { }
                
               
                return score_Max;
        }


        //Sinh ngẫu nhiên vị trí quả
        int typefruit = 0;
        void randonQua()
        {
            typefruit = rand.Next(1, 3);
            fruitX = rand.Next(1, width - 1);
            fruitY = rand.Next(1, height - 1);
        }

        //Cập nhật màn hình
        void Update()
        {
            while (!gameOver)
            {
                CheckInput();
                Logic();
                Render();
                if (reset) break;
                //DUng man hinh 1s
                Thread.Sleep(200);
                if (score >= 100)
                {
                    Thread.Sleep(50);
                }
                else if (score >=200)
                {
                    Thread.Sleep(50);
                }
            }
            if (gameOver) Lose();
        }

        //Điều khiển phím
        void CheckInput()
        {
            while (Console.KeyAvailable)
            {
                //Cho bam phim bat ky
                keypress = Console.ReadKey(true);
                //dir -> pre_dir
                pre_dir = dir;  //luu lai huong di truoc do
                switch (keypress.Key)
                {
                    case ConsoleKey.Q: Environment.Exit(0); break;
                    case ConsoleKey.P: dir = "STOP"; break;
                    case ConsoleKey.A: dir = "LEFT"; break;
                    case ConsoleKey.D: dir = "RIGHT"; break;
                    case ConsoleKey.W: dir = "UP"; break;
                    case ConsoleKey.S: dir = "DOWN"; break;
                }
            }

        }

        //Kiểm tra điều hướng
        void Logic()
        {
            int preX = TailX[0], preY = TailY[0]; // (x,y)
            int tempX, tempY;
            //0 1 2 3 4 => Con rắn ăn thêm quà            //x 0 1 2 3 4
            if (dir != "STOP")
            {
                TailX[0] = headX; TailY[0] = headY;

                for (int i = 1; i < nTail; i++)
                {
                    tempX = TailX[i]; tempY = TailY[i];
                    TailX[i] = preX; TailY[i] = preY;
                    preX = tempX; preY = tempY;
                }
            }
            switch (dir)
            {
                case "RIGHT": headX++; break;
                case "LEFT": headX--; break;
                case "DOWN": headY++; break;
                case "UP": headY--; break;
                case "STOP":
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Game pause!");
                            Console.WriteLine("- Press P key to PAUSE game");
                            Console.WriteLine("- Press E key to RESET game");
                            Console.WriteLine("- Press Q key to QUIT game");

                            keypress = Console.ReadKey(true);
                            if (keypress.Key == ConsoleKey.Q)
                                Environment.Exit(0);
                            if (keypress.Key == ConsoleKey.R)
                            {
                                reset = true; break; //choi lai game
                            }
                            if (keypress.Key == ConsoleKey.P)
                                break;  //choi tiep game
                        }
                    }
                    dir = pre_dir; break;
            }
            //kiem tra va cham bien (le trai, phai)
            if (headX <= 0 || headX >= width - 1 ||
                headY <= 0 || headY >= height - 1) gameOver = true;
            else gameOver = false;
            //kiem tra an qua
            
            if (headX == fruitX && headY == fruitY)
            {
                
                score += 5*typefruit; nTail++;    //tinh diem khi an qua
                

                randonQua();            //random diem qua moi

            }
            if (((dir == "LEFT" && pre_dir != "UP") && (dir == "LEFT" && pre_dir != "DOWN")) ||
                ((dir == "RIGHT" && pre_dir != "UP") && (dir == "RIGHT" && pre_dir != "DOWN")))
                horizontal = true;
            else horizontal = false;

            if (((dir == "UP" && pre_dir != "LEFT") && (dir == "UP" && pre_dir != "RIGHT")) ||
                ((dir == "DOWN" && pre_dir != "LEFT") && (dir == "DOWN" && pre_dir != "RIGHT")))
                vertical = true;
            else vertical = false;

            //kiem tra cai dau va cham than con ran
            for (int i = 1; i < nTail; i++)
            {
                if (headX == TailX[i] && headY == TailY[i])
                {
                    if (horizontal || vertical) gameOver = false; //quay dau 180 độ [bẫy lỗi quay đầu]
                    else gameOver = true;
                }
                if (fruitX == TailX[i] && fruitY == TailY[i]) //quà trùng thân con rắn cho random lại
                    randonQua();
            }
        }

        //Hiển thị màn hình
        void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1)   //viền khung trên và dưới
                        Console.Write("#");
                    else if (j == 0 || j == width - 1) //viền khung trái và phải
                        Console.Write("#");
                    else if (j == fruitX && i == fruitY) //hộp quả 
                        switch (typefruit)
                        {
                            case 1: Console.Write("1");break;
                            case 2: Console.Write("2");break;
                        }
                    else if (j == headX && i == headY) //đầu con rắn
                    { Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.Green; }
                    else
                    {
                        isprinted = false;
                        for (int k = 0; k < nTail; k++)
                        {

                            if (TailX[k] == j && TailY[k] == i)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("o");  //thân con rắn
                                Console.ForegroundColor = ConsoleColor.Green;
                                isprinted = true;
                            }
                        }
                        if (!isprinted) Console.Write(" "); //vị trí còn lại
                    }
                }
                Console.WriteLine();
            }
            //Hiển thị panel thông tin điểm phía dưới khung viền
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Your score: " + score);
            Console.WriteLine("Your score MAX : " + Max_Score() );
        }

        //Xử lý khi thua
        void Lose()
        {
            Console.WriteLine("YOU DIED");
            Console.WriteLine("      - Press E key to RESET game");
            Console.WriteLine("      - Press Q key to QUIT game");
            //save max score
            if (score >score_Max)
            {
                score_Max = score;
            }
            string save_Score = "score.txt";
            File.WriteAllText(save_Score, score_Max.ToString());
            while (true)
            {
                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.Q)
                    Environment.Exit(0);
                if (keypress.Key == ConsoleKey.E)
                    reset = true; break;
            }
        }

        static void Main(string[] args)
        {
            SnakeGameNe snakegame = new SnakeGameNe(); //game ko xac dinh diem dung
            snakegame.ShowBanner();
            while (true)
            {
                snakegame.Setup();
                snakegame.Update();
                Console.Clear(); //Xoa man hinh hien thi
            }
        }
    }
}
