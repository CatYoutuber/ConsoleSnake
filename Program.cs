using System;
using System.Threading;
using System.Collections.Generic;
using SnakeExtensions;

namespace Snake
{
    class Program
    {
        static List<Block> snakeBlocks = new List<Block>();
        public const string PLAYER = "░", PLAYERHEAD = "▓", FOOD = "■";
        static Direction direction = Direction.Right;
        static Block food;
        static Random random;
        static int score = 0;
        static bool playing = true, GameOver = false, PickUpFood = false;
        static readonly string COPYRIGHT =
            "\nCONTROLS  W     F2 -    \n" +
            "        A S D   Pause   \n" +
            "------------------------\n" +
            " Colored Console Snake  \n" +
            "(C)CatYoutuber 2018-2021\n" +
            "------------------------\n";
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(FoodGetBeepLoop)).Start();
            Console.Title = "Snake";
            snakeBlocks.Add(new Block(2, 2));
            snakeBlocks.Add(new Block(3, 2));
            snakeBlocks.Add(new Block(4, 2));
            food = new Block(1, 1);
            random = new Random();
            while (true)
            {
                string[][] field = Field.GetField();
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(false).Key;
                    if (key == ConsoleKey.W && direction != Direction.Down)
                        direction = Direction.Up;
                    if (key == ConsoleKey.S && direction != Direction.Up)
                        direction = Direction.Down;
                    if (key == ConsoleKey.D && direction != Direction.Left)
                        direction = Direction.Right;
                    if (key == ConsoleKey.A && direction != Direction.Right)
                        direction = Direction.Left;
                    if (key == ConsoleKey.F2)
                        playing = !playing;
                }
                Console.Clear();
                if (playing)
                {
                    field = Field.GetField();
                    switch (direction)
                    {
                        case Direction.Up:
                            snakeBlocks.Add(new Block(snakeBlocks[snakeBlocks.Count - 1].X, snakeBlocks[snakeBlocks.Count - 1].Y - 1));
                            break;
                        case Direction.Down:
                            snakeBlocks.Add(new Block(snakeBlocks[snakeBlocks.Count - 1].X, snakeBlocks[snakeBlocks.Count - 1].Y + 1));
                            break;
                        case Direction.Right:
                            snakeBlocks.Add(new Block(snakeBlocks[snakeBlocks.Count - 1].X + 1, snakeBlocks[snakeBlocks.Count - 1].Y));
                            break;
                        case Direction.Left:
                            snakeBlocks.Add(new Block(snakeBlocks[snakeBlocks.Count - 1].X - 1, snakeBlocks[snakeBlocks.Count - 1].Y));
                            break;
                        default:
                            break;
                    }
                    if (snakeBlocks[snakeBlocks.Count - 1] != food)
                        snakeBlocks.Remove(snakeBlocks[0]);
                    else
                    {
                        PickUpFood = true;
                        score++;
                        bool FoodOnEmptyPlace = false;
                        food = new Block(random.Next(1, field[0].Length - 2), random.Next(1, field.Length - 2));
                        while (true)
                        {
                            foreach(Block block in snakeBlocks)
                            {
                                if (block == food)
                                {
                                    food = new Block(random.Next(1, field[0].Length - 2), random.Next(1, field.Length - 2));
                                    FoodOnEmptyPlace = true;
                                    foreach (Block block2 in snakeBlocks)
                                        if (block2 == food)
                                        {
                                            FoodOnEmptyPlace = false;
                                            break;
                                        }
                                        else FoodOnEmptyPlace = true;
                                    break;
                                }
                                else FoodOnEmptyPlace = true;
                            }
                            if (FoodOnEmptyPlace) break;
                        }
                    }
                    if (snakeBlocks[snakeBlocks.Count - 1].X > field[0].Length - 2 ||
                       snakeBlocks[snakeBlocks.Count - 1].X < 1 ||
                       snakeBlocks[snakeBlocks.Count - 1].Y > field.Length - 2 ||
                       snakeBlocks[snakeBlocks.Count - 1].Y < 1)
                    {
                        score = 0;
                        playing = false;
                        GameOver = true;
                        new Thread(new ThreadStart(FinishBeep)).Start();
                    }
                }
                foreach (Block block in snakeBlocks)
                    field[block.Y][block.X] = PLAYER;
                field[food.Y][food.X] = FOOD;
                field[snakeBlocks[snakeBlocks.Count - 1].Y][snakeBlocks[snakeBlocks.Count - 1].X] = PLAYERHEAD;
                Console.ForegroundColor = ConsoleColor.Blue;
                if (!playing)
                    if (GameOver)
                        Console.WriteLine("----GAME OVER!----");
                    else
                        Console.WriteLine("------PAUSED------");
                else
                    Console.WriteLine("Score: " + score);

                foreach (string[] line in field)
                {
                    foreach (string pixel in line)
                    {
                        switch (pixel)
                        {
                            case PLAYER:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case PLAYERHEAD:
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case FOOD:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }
                        Console.Write(pixel);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(Environment.NewLine);
                }
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(COPYRIGHT);
                Thread.Sleep(750);
            }
        }
        public static void FinishBeep()
        {
            Console.Beep(750, 500);
            Thread.Sleep(150);
            Console.Beep(500, 500);
            Thread.Sleep(150);
            for (int c = 0; c < 5; c++)
                Console.Beep(300, 150);
            Environment.Exit(0x1);
        }
        public static void FoodGetBeepLoop()
        {
            while (true)
            {
                if (PickUpFood)
                {
                    PickUpFood = false;
                    Console.Beep(750, 200);
                    Console.Beep(1000, 300);
                }
            }
        }
    
    }  
}
