using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleGame2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Gray;
            Matrix m = new Matrix();
            m.FillMatrix();
            Render r = new Render();
            r.FillMatrix();
            Render.Group();
            MoveHero.Process();      
        }
    }
    abstract class PlayField
    {
        public struct Cell
        {
            char symbol;
            public char Symbol
            {
                get { return symbol; }
                set { symbol = value; }
            }
            public Cell(char symbol)
            {
                this.symbol = symbol;
            }
        }
        static Cell[,] field;
        public static Cell[,] Field
        {
            get { return field; }
            set { field = value; }
        }
    }
    class Matrix : PlayField
    {
         public static char[,] charField = new char[,] {
            {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'},
            {'#','☻',' ','o',' ','◄',' ','o',' ',' ',' ','o',' ','o',' ',' ',' ',' ',' ',' ','#',' ',' ','o','#',' ',' ',' ','o',' ',' ',' ',' ','o','#'},
            {'#',' ','#','#','#',' ','#','#','#','#','#','#','#',' ','#','#','#',' ','#','o','#','o','#',' ','#',' ','#','#','#','#','#','#','#',' ','#'},
            {'#',' ','#','Ø','#','o','#',' ','o',' ','#','~',' ','o','#','Ø','#',' ','#','Ø','#',' ','#',' ',' ',' ','#','♦','#',' ',' ',' ','#','◄','#'},
            {'#',' ','#',' ','#',' ','#',' ','#','o','#','#','#','#','#',' ','#',' ','#','#','#',' ','#','#','#','#','#','~','#',' ','#','o','#',' ','#'},
            {'#',' ','~',' ','#','♥',' ','o','#',' ','o',' ','o',' ','~',' ','#','o',' ',' ','o',' ',' ',' ',' ','o',' ','~','#',' ','#','~','#','b','#'},
            {'#',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','#','#','#',' ','#','#','#','#','#'},
            {'#',' ',' ',' ',' ','o',' ','o',' ',' ','o',' ','#','o',' ',' ',' ','o',' ',' ',' ','o',' ',' ','#',' ',' ',' ',' ','o',' ','~',' ','o','#'},
            {'#','#','#','o','#',' ','#','#','#','#','#','o','#',' ','#','o','#','#','#','#','#',' ','#',' ','#','#','#','#','#','#','#','#','#',' ','#'},
            {'#',' ',' ',' ','#',' ',' ','o',' ',' ','#','o',' ','?','#',' ',' ',' ',' ',' ','#','~','#',' ',' ',' ','o',' ','#',' ',' ',' ','#',' ','#'},
            {'#',' ','#',' ','#',' ','#','#','#','◄','#','#','#','#','#','~','#','#','#','o','#','#','#',' ','#','#','#',' ','#','o','#',' ','#','o','#'},
            {'#','o','#',' ','#','Ø','#','~','#',' ',' ',' ','▲',' ','#','o','#',' ','#',' ',' ',' ','#',' ','o',' ','#','~',' ',' ','#','o','#',' ','#'},
            {'#',' ','#','o','#','#','#',' ','#',' ','#','#','#','o','#','o','#',' ','#','#','#',' ','#',' ','#',' ','#','#','#','#','#',' ','#','~','#'},
            {'#',' ','#',' ','♦',' ',' ',' ','#','o',' ',' ','#',' ','#','Ø','#','◄','#','~','#',' ','#','o','#',' ',' ',' ',' ',' ','#',' ','#',' ','#'},
            {'#','~','#',' ','#',' ','#','o','#',' ','#',' ','#',' ','#','#','#',' ','#',' ','#','o','#',' ','#','#','#','#','#',' ','#','♥','#','B','#'},
            {'#','B','#','~','#',' ','#','~','#',' ','#','o','#',' ','o',' ',' ',' ',' ',' ','#','Ø','#',' ','▲','o',' ','~','#','?','#','Ø','#','o','#'},
            {'#','o','#','#','#',' ','#','#','#',' ','#',' ','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','#','#','#',' ','#','#','#','B','#'},
            {'#','~',' ','o','#',' ','#',' ',' ','o','#',' ',' ',' ',' ',' ',' ','o','#',' ',' ',' ','#','▲','#',' ','#','~','#',' ',' ',' ',' ',' ','#'},
            {'#','#','#','o','#',' ','#',' ','#',' ','#','#','#','#','#','#','#',' ','#',' ','#',' ','#',' ','#',' ','#',' ','#','#','#','#','#','#','#'},
            {'#','▲','o','o','#','♥','#',' ','#',' ',' ','o','B','o','o','♥','#','▲',' ',' ','#','o',' ',' ','#',' ',' ','▲',' ','o',' ','♦',' ','♥','#'},
            {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'},
            };

        public static int rows = charField.GetUpperBound(0) + 1;
        public static int columns = charField.Length / rows;
        public static Cell[,] FieldCell = new Cell[rows, columns];

        public char this[int i, int k]
        {
            set { charField[i, k] = value; }
            get { return charField[i, k]; }
        }

        public  virtual void FillMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    FieldCell[i, j] = new Cell(charField[i, j]);
                }
            }
        }
    }
    class Render:Methods
    {
        public static void Group()
        {
            PrintCoins();
            PrintLives();
            PrintMoves();
            GetScore();
            PrintScore();
            PrintCrystals();
            string[] info = { "☻ - ГЛАВНЫЙ ГЕРОЙ ", "◄ - ИНОГДА РАБОТАЮЩИЙ ТЕЛЕПОРТ", "# - СТЕНА ", "o - МОНЕТКА","? - СЕКРЕТНЫЙ ПРИЗ", "♥ - ЖИЗНЬ", "♦ - КРИСТАЛЛ", "~ - УМЕНЬШЕНИЕ ЖИЗНЕЙ ", "▲ - ТЕЛЕПОРТ В СЛУЧАЙНОЕ МЕСТО", "Ø - ЛОВУШКА(ПРОИГРЫШ) ", "b - КЛЮЧ", "B - ДВЕРЬ ","+ - ПРИЗ(ДОПОЛНИТЕЛЬНАЯ ЖИЗНЬ)", "→ - ВЫХОД" };
            for(int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(40, i+7);
                Console.WriteLine(info[i]);
            }
            
        }
        public override void FillMatrix()
        {
            Console.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (FieldCell[i, j].Symbol == '#')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(FieldCell[i, j].Symbol);
                    }

                    else if (FieldCell[i, j].Symbol == 'o')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(FieldCell[i, j].Symbol);
                    }
                    else if (FieldCell[i, j].Symbol == '♥' || FieldCell[i, j].Symbol == '◄')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(FieldCell[i, j].Symbol);
                    }
                    else if (FieldCell[i, j].Symbol == 'Ø' || FieldCell[i, j].Symbol == '~' || FieldCell[i, j].Symbol == '☻')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(FieldCell[i, j].Symbol);
                    }
                    else if (FieldCell[i, j].Symbol == '▲' || FieldCell[i, j].Symbol == '?')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(FieldCell[i, j].Symbol);
                    }
                    else if (FieldCell[i, j].Symbol == 'b' || FieldCell[i, j].Symbol == 'B' || FieldCell[i, j].Symbol == '♦')
                    {
                        Console.SetCursorPosition(j, i);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(FieldCell[i, j].Symbol);
                    }
                }
                Console.WriteLine();
            }
        }
    }
    class Methods:Matrix
    {
        static public int coin = 0;
        static public int lives = 3;
        static public int moves = 0;
        static public int score = 0;
        static public int crystals = 0;
        static public int times = 0;
        static public bool gameStatus = true;

        public static void Win()
        {
            Console.Clear();
            Console.WriteLine("YOU WIN!");
            Console.Beep(1000, 600);
            gameStatus = false;
        }

        public static void Lose()
        {
            Console.Clear();
            Console.WriteLine("YOU LOSE!");
            Console.Beep(200, 700);
            gameStatus = false;
            Console.ReadKey();
        }

        public static void FindDoor()
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (FieldCell[x, y].Symbol == 'B')
                    {
                        Console.SetCursorPosition(y, x);
                        Console.Write(FieldCell[x, y].Symbol = ' ');
                    }
                }
            }
        }

        public static void Teleport()
        {
            Random x = new Random();
            while (true)
            {
                int i = x.Next(0, rows);
                int j = x.Next(0, columns);
                if (FieldCell[i, j].Symbol == ' ')
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(FieldCell[i, j].Symbol = '☻');
                    break;
                }
            }
        }

        public static void TeleportSometimes(int first, int second)
        {
            ++times;
            if(times%3==0)
            {
                Teleport();
            }
            else
            {
                if (FieldCell[first, second].Symbol != '#')
                {
                    Console.SetCursorPosition(second, first);
                    Console.Write(FieldCell[first, second].Symbol = '☻');
                }
                else { Teleport(); }
            }
        }

        public static void GetSecretPrize()
        {
            int[] arr = { coin, lives };
            Random random = new Random();
            int randomForArr = random.Next(0, 2);
            if(randomForArr == 0)
            {
                int plus = random.Next(5, 21);
                Console.SetCursorPosition(66, 7);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"ВЫ ПОЛУЧИЛИ {plus+1} ДОПОЛНИТЕЛЬНЫХ МОНЕТОК!   ");
                coin += plus;
                PrintCoins();
                GetScore();
                PrintScore();
            }
            else if(randomForArr == 1)
            {
                int plus = random.Next(2, 4);
                Console.SetCursorPosition(66, 7);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"ВЫ ПОЛУЧИЛИ {plus} ДОПОЛНИТЕЛЬНЫХ ЖИЗНИ!    ");
                lives += plus;
                PrintLives();
                GetScore();
                PrintScore();
            }
        }

        public static void GetPrize()
        {
            Random x = new Random();
            while (true)
            {
                int i = x.Next(0, rows);
                int j = x.Next(0, columns);
                if (FieldCell[i, j].Symbol == ' ')
                {
                    Console.SetCursorPosition(j, i);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(FieldCell[i, j].Symbol = '+');
                    break;
                }
            }
        }

        public static void PrintCoins()
        {
            Console.SetCursorPosition(40, 0);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"КОЛИЧЕСТВО МОНЕТ - {coin}");
            coin++;
        }

        public static void PrintCrystals()
        {
            Console.SetCursorPosition(40, 4);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"КОЛИЧЕСТВО КРИСТАЛЛОВ - {crystals}");
            crystals++;
        }

        public static void PrintLives()
        {
            Console.SetCursorPosition(40, 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"КОЛИЧЕСТВО ЖИЗНЕЙ - {lives}");
        }

        public static void PrintScore()
        {
            Console.SetCursorPosition(40, 3);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"ВАШ СЧЁТ - {score}");
        }

        public static void GetScore()
        {
            score = (coin * 5 + lives * 10 + crystals*50) - 35;
        }

        public static void CheckScore()
        {
            if(score>=200)
            {
                Win();
                Console.SetCursorPosition(0, 3);
                Console.WriteLine("ПОЗДАРВЛЯЕМ!!!  ВЫ НАБРАЛИ 200+ ОЧКОВ! ");
            }
        }
        public static void GetLives()
        {
            --lives;
            if (lives > 0)
            {
                PrintLives();
            }
            else if (lives == 0)
            {
                PrintLives();
                Lose();
            }
        }

        public static void PrintMoves()
        {
            Console.SetCursorPosition(40, 2);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"КОЛИЧЕСТВО ХОДОВ - {moves}");
            moves++;
        }

        public static void PrintPrize()
        {
            Console.SetCursorPosition(65, 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("ВЫ ПОЛУЧИЛИ ПРИЗ - ЕЩЕ ОДНУ ЖИЗНЬ!");
        }
    }
    class MoveHero:Methods
    {
        public static void moveHero(ConsoleKeyInfo keyInfo)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (FieldCell[i, j].Symbol == '☻')
                    {
                        if (keyInfo.Key == ConsoleKey.UpArrow)
                        {
                            if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == ' ')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '→')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                Win();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == 'b')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                FindDoor();
                                GetPrize();
                                Console.SetCursorPosition(34, 15);
                                Console.Write(FieldCell[15, 34].Symbol = '→');
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '▲')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = ' ');
                                Teleport();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '◄')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                TeleportSometimes(i-2,j);
                                return;
                            }

                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '~')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                GetLives();
                                GetScore();
                                CheckScore();
                                PrintScore();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == 'o')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                PrintCoins();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '?')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                Console.Beep(250, 100);
                                GetSecretPrize();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '♥')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '♦')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                if (crystals == 3) { Win(); }
                                PrintCrystals();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == 'Ø')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Lose();
                                return;
                            }
                            else if ((i - 1) >= 0 && FieldCell[i - 1, j].Symbol == '+')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i - 1);
                                Console.Write(FieldCell[i - 1, j].Symbol = '☻');
                                PrintPrize();
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                        }

                        else if (keyInfo.Key == ConsoleKey.DownArrow)
                        {
                            if ((i + 1) <= rows && FieldCell[i + 1, j].Symbol == ' ')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                return;
                            }
                            else if ((i + 1) <= rows && FieldCell[i + 1, j].Symbol == '→')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                Win();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == 'b')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                FindDoor();
                                GetPrize();
                                Console.SetCursorPosition(34, 15);
                                Console.Write(FieldCell[15, 34].Symbol = '→');
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '▲')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = ' ');
                                Teleport();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '◄')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                TeleportSometimes(i + 2, j);
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '~')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                GetLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == 'o')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                PrintCoins();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '?')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                Console.Beep(250, 100);
                                GetSecretPrize();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '♥')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '♦')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                if (crystals == 3) { Win(); }
                                PrintCrystals();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == 'Ø')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Lose();
                                return;
                            }
                            else if ((i + 1) >= 0 && FieldCell[i + 1, j].Symbol == '+')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j, i + 1);
                                Console.Write(FieldCell[i + 1, j].Symbol = '☻');
                                PrintPrize();
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                        }

                        else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        {
                            if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == ' ')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '→')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                Win();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == 'b')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                FindDoor();
                                GetPrize();
                                Console.SetCursorPosition(34, 15);
                                Console.Write(FieldCell[15, 34].Symbol = '→');
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '▲')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = ' ');
                                Teleport();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '◄')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                TeleportSometimes(i, j - 2);
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '~')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                GetLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == 'o')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                PrintCoins();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '?')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                Console.Beep(250, 100);
                                GetSecretPrize();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '♥')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '♦')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                if (crystals == 3) { Win(); }
                                PrintCrystals();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == 'Ø')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Lose();
                                return;
                            }
                            else if ((j - 1) >= 0 && FieldCell[i, j - 1].Symbol == '+')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j - 1, i);
                                Console.Write(FieldCell[i, j - 1].Symbol = '☻');
                                PrintPrize();
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                        }

                        else if (keyInfo.Key == ConsoleKey.RightArrow)
                        {
                            if ((j + 1) <= columns && FieldCell[i, j + 1].Symbol == ' ')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                return;
                            }
                            else if ((j + 1) <= columns && FieldCell[i, j + 1].Symbol == '→')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                Win();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == 'b')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                FindDoor();
                                GetPrize();
                                Console.SetCursorPosition(34, 15);
                                Console.Write(FieldCell[15, 34].Symbol = '→');
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '▲')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = ' ');
                                Teleport();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '◄')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                TeleportSometimes(i, j + 2);
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '~')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                GetLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == 'o')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                PrintCoins();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '?')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                Console.Beep(250, 100);
                                GetSecretPrize();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '♥')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '♦')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                if (crystals == 3) { Win(); }
                                PrintCrystals();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == 'Ø')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Lose();
                                return;
                            }
                            else if ((j + 1) >= 0 && FieldCell[i, j + 1].Symbol == '+')
                            {
                                PrintMoves();
                                Console.SetCursorPosition(j, i);
                                Console.Write(FieldCell[i, j].Symbol = ' ');
                                Console.SetCursorPosition(j + 1, i);
                                Console.Write(FieldCell[i, j + 1].Symbol = '☻');
                                PrintPrize();
                                ++lives;
                                PrintLives();
                                CheckScore();
                                GetScore();
                                PrintScore();
                                return;
                            }
                        }
                    }
                }
            }
        }
        public static void Process()
        {
            while (gameStatus)
            {
                var keyInfo = Console.ReadKey(true);
                moveHero(keyInfo);
            }
        }
    }
}