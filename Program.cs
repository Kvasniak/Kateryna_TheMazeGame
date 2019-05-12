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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Пожалуйста, введите свое имя ");
            string userName = Console.ReadLine();
            bool status = true;
            do
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"{userName},выберите один пункт из меню, нажав соответствующую клавишу");
                Console.WriteLine("   МЕНЮ");
                Console.WriteLine("1. Играть\n2. Правила игры\n3. Выход\n ");
                int choice = int.Parse(Console.ReadLine());
                
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Matrix m = new Matrix();
                        m.FillMatrix();
                        Render r = new Render();
                        r.FillMatrix();
                        Render.Group();
                        MoveHero.Process();
                        status = false;
                        break;
                    case 2:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("Правила игры: \n Цель игры - как можно быстрее выбраться из лабиринта. Во время игры  герой должен собирать " +
                            "монетки, жизни, кристаллы, чтобы получить максимальное количество очков. Также игрок должен найти ключ, чтобы открыть дверь к выходу." +
                            $"Но не все так просто, как кажется! {userName}, Вас ожидают смертельные ловушки, коварные призы и уменьшение очков. Поэтому, для того, чтобы выбраться из " +
                            "лабиринта, нужно собирать как можно больше жизней.");
                        break;
                    case 3:
                        Console.Clear();
                        Environment.Exit(0);
                        status = false;
                        break;
                    default:
                        Console.WriteLine("Ошибка");
                        break;
                }
            }
            while (status);
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
            {'#','☻',' ','o',' ',' ','◄',' ',' ',' ',' ','o',' ','o',' ',' ',' ',' ',' ',' ','#',' ',' ','o','#',' ',' ',' ','o',' ',' ',' ',' ','o','#'},
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
            {'#',' ','#',' ','♦',' ',' ',' ','#','o',' ',' ','#',' ','#','Ø','#','◄','#','~','#',' ','#','o','#',' ','~',' ',' ',' ','#',' ','#',' ','#'},
            {'#','~','#',' ','#',' ','#','o','#',' ','#',' ','#',' ','#','#','#',' ','#',' ','#','o','#',' ','#','#','#','#','#',' ','#','♥','#','B','#'},
            {'#','B','#','~','#',' ','#','~','#',' ','#','o','#',' ','o',' ',' ',' ',' ',' ','#','Ø','#',' ','▲','o',' ','~','#','?','#','Ø','#',' ','#'},
            {'#','o','#','#','#',' ','#','#','#',' ','#',' ','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','#','#','#',' ','#','#','#','B','#'},
            {'#','~',' ','o','#',' ','#',' ',' ','o','#',' ',' ',' ',' ',' ',' ','o','#',' ',' ',' ','#','▲','#',' ','#','~','#','~',' ',' ','~',' ','#'},
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

        public virtual void FillMatrix()
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
    class Render : Methods
    {
        
        public static void Group()
        {
            PrintCoins();
            PrintLives();
            PrintMoves();
            GetScore();
            PrintScore();
            PrintCrystals();
            string[] info = { "☻ - ГЛАВНЫЙ ГЕРОЙ ", "◄ - ИНОГДА РАБОТАЮЩИЙ ТЕЛЕПОРТ", "# - СТЕНА ", "o - МОНЕТКА", "? - СЕКРЕТНЫЙ ПРИЗ", "♥ - ЖИЗНЬ", "♦ - КРИСТАЛЛ", "~ - УМЕНЬШЕНИЕ ЖИЗНЕЙ ", "▲ - ТЕЛЕПОРТ В СЛУЧАЙНОЕ МЕСТО", "Ø - ЛОВУШКА(ПРОИГРЫШ) ", "b - КЛЮЧ", "B - ДВЕРЬ ", "+ - ПРИЗ(ДОПОЛНИТЕЛЬНАЯ ЖИЗНЬ)", "→ - ВЫХОД" };
            for (int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(40, i + 7);
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
    class Methods : Matrix
    {
        static public int coin = 0;
        static public int lives = 3;
        static public int moves = 0;
        static public int score = 0;
        static public int crystals = 0;
        static public int times = 0;
        static public int keys = 0;
        static public bool gameStatus = true;

        public static void Win()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Вы выиграли!");
            Console.Beep(1000, 600);
            gameStatus = false;
            Console.ReadKey();
        }

        public static void Lose()
        {
            Console.Clear();
            Console.WriteLine("К сожалению, Вы проиграли!");
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
            if (times % 3 == 0)
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
            if (randomForArr == 0)
            {
                int plus = random.Next(5, 21);
                Console.SetCursorPosition(66, 7);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"ВЫ ПОЛУЧИЛИ {plus + 1} ДОПОЛНИТЕЛЬНЫХ МОНЕТОК!   ");
                coin += plus;
                PrintCoins();
                GetScore();
                PrintScore();
            }
            else if (randomForArr == 1)
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
            Console.WriteLine($"КОЛИЧЕСТВО  МОНЕТ - {coin}");
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
            if(!gameStatus)
            {
                Console.Clear();
            }
        }

        public static void GetScore()
        {
            score = (coin * 5 + lives * 10 + crystals * 50) - 35;
        }

        public static void CheckScore()
        {
            if (score >= 500)
            {
                Win();
                Console.SetCursorPosition(0, 3);
                Console.WriteLine("ПОЗДАРВЛЯЕМ!!!  ВЫ НАБРАЛИ 500+ ОЧКОВ! ");
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

        public static void WriteEmptyLine(int i, int j)
        {
            Console.SetCursorPosition(j, i);
            Console.Write(FieldCell[i, j].Symbol = ' ');
        }

        public static void WriteHero(int j, int change)
        {
            Console.SetCursorPosition(j, change);
            Console.Write(FieldCell[change, j].Symbol = '☻');
        }

        public static void WriteExit()
        {
            Console.SetCursorPosition(34, 15);
            Console.Write(FieldCell[15, 34].Symbol = '→');
        }

        public static void FindOutScore()
        {
            GetScore();
            CheckScore();
            PrintScore();
        }

        public static void CheckMoves()
        {
            if(keys==1 && moves>70)
            {
                Console.SetCursorPosition(33, 14);
                if (FieldCell[14, 33].Symbol == ' ')
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(FieldCell[14, 33].Symbol = 'B');
                }
            }
        }

        public static void CheckCoordinatesUpDown(int changes, int x, int y, int telep)
        {
            if (changes >= 0 && FieldCell[changes, y].Symbol == ' ')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '→')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                Win();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == 'b')
            {
                keys++;
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                FindDoor();
                GetPrize();
                WriteExit();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '▲')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteEmptyLine(changes,y);
                Teleport();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '◄')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                TeleportSometimes(telep, y);
                CheckMoves();
                return;
            }

            else if (changes >= 0 && FieldCell[changes, y].Symbol == '~')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                GetLives();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == 'o')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                PrintCoins();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '?')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                Console.Beep(250, 100);
                GetSecretPrize();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '♥')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                ++lives;
                PrintLives();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '♦')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                if (crystals == 3) { Win(); }
                PrintCrystals();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == 'Ø')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                Lose();
                return;
            }
            else if (changes >= 0 && FieldCell[changes, y].Symbol == '+')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(y, changes);
                PrintPrize();
                ++lives;
                PrintLives();
                FindOutScore();
                CheckMoves();
                return;
            }
        }

        public static void CheckCoordinatesLeftRight(int changes, int x, int y, int telep)
        {
            if (changes >= 0 && FieldCell[x, changes].Symbol == ' ')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes,x);
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '→')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                Win();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == 'b')
            {
                keys++;
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                FindDoor();
                GetPrize();
                WriteExit();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '▲')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteEmptyLine(x, changes);
                Teleport();
                CheckMoves();
                return;
            }

            else if (changes >= 0 && FieldCell[x, changes].Symbol == '◄')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                TeleportSometimes(x, telep);
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '~')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                GetLives();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == 'o')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                PrintCoins();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '?')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                Console.Beep(250, 100);
                GetSecretPrize();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '♥')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                ++lives;
                PrintLives();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == '♦')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                if (crystals == 3) { Win(); }
                PrintCrystals();
                FindOutScore();
                CheckMoves();
                return;
            }
            else if (changes >= 0 && FieldCell[x, changes].Symbol == 'Ø')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                Lose();
                return;
            }

            else if (changes >= 0 && FieldCell[x, changes].Symbol == '+')
            {
                PrintMoves();
                WriteEmptyLine(x, y);
                WriteHero(changes, x);
                PrintPrize();
                ++lives;
                PrintLives();
                FindOutScore();
                CheckMoves();
                return;
            }
        }
    }
    class MoveHero : Methods
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
                            CheckCoordinatesUpDown(i - 1, i, j, i - 2);
                            return;
                        }

                        else if (keyInfo.Key == ConsoleKey.DownArrow)
                        {
                         CheckCoordinatesUpDown(i + 1, i, j, i + 2);
                          return;
                        }

                        else if (keyInfo.Key == ConsoleKey.LeftArrow)
                        {
                            CheckCoordinatesLeftRight(j - 1, i, j, j - 2);
                            return;
                        }

                        else if (keyInfo.Key == ConsoleKey.RightArrow)
                        {
                            CheckCoordinatesLeftRight(j + 1, i, j, j + 2);
                            return;
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