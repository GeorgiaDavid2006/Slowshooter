using System;
using System.Data;
using System.Linq;

namespace Slowshooter
{
    internal class Program
    {

        static string playField = 
@"+---+   +---+
|   |   |   |
|   |   |   |
|   |   |   |
+---+   +---+";

        static bool isPlaying = true;

        // player input 
        static int p1_x_input;
        static int p1_y_input;

        static int p2_x_input;
        static int p2_y_input;

        // player 1 pos
        static int p1_x_pos = 2;
        static int p1_y_pos = 2;

        // player 2 pos
        static int p2_x_pos = 10;
        static int p2_y_pos = 2;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 3);
        static (int, int) p1_min_max_y = (1, 3);
        static (int, int) p2_min_max_x = (9, 11);
        static (int, int) p2_min_max_y = (1, 3);

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // health
        static int p1Health = 5;
        static int p2Health = 5;

        // Random

        static int randXp1 = 0;
        static int randYp1 = 0;

        static int randXp2 = 0;
        static int randYp2 = 0;

        static int healthXp1 = 0;
        static int healthYp1 = 0;

        static int healthXp2 = 0;
        static int healthYp2 = 0;

        static Random xAxis = new Random();
        static Random yAxis = new Random();

        static int p1AmmoBoard = 0;
        static int p2AmmoBoard = 0;

        static int p1HealthBoard = 0;
        static int p2HealthBoard = 0;

        static bool p1Shoot = false;
        static bool p2Shoot = false;

        //ammo
        static int p1Ammo = 0;
        static int p2Ammo = 0;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[]{ 'W', 'A', 'S', 'D', ' ' }, new char[]{ 'J', 'I', 'L', 'K', ' ' });
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while(isPlaying)
            {
                ProcessInput();
                Update();
                Draw();
                
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("GAME OVER");
            Console.ForegroundColor = ConsoleColor.White;

            if (p1Health > 0)
            {
                Console.ForegroundColor = playerColors[0];
                Console.WriteLine("P1 HAS WON");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = playerColors[1];
                Console.WriteLine("P2 HAS WON");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = allKeybindings.Item1;
            else allowedKeysThisTurn = allKeybindings.Item2;

            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            while (!allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys 
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;
          
            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;

            if(input == ConsoleKey.Spacebar )
            {
                if(turn % 2 == 0 && p1Ammo > 0)
                {
                    p1Shoot = true;
                }
                else if(turn % 2 != 0 && p2Ammo > 0)
                {
                    p2Shoot = true;
                }
            }

        }

        static void Update()
        {
            // update players' positions based on input
            p1_x_pos += p1_x_input;
            p1_x_pos = p1_x_pos.Clamp(p1_min_max_x.Item1, p1_min_max_x.Item2);

            p1_y_pos += p1_y_input;
            p1_y_pos = p1_y_pos.Clamp(p1_min_max_y.Item1, p1_min_max_y.Item2);

            p2_x_pos += p2_x_input;
            p2_x_pos = p2_x_pos.Clamp(p2_min_max_x.Item1, p2_min_max_x.Item2);

            p2_y_pos += p2_y_input;
            p2_y_pos = p2_y_pos.Clamp(p2_min_max_y.Item1, p2_min_max_y.Item2);

            turn += 1;

            //Spawnn random area
            //Ammo
            if (p1Ammo <= 1 && p1AmmoBoard == 0)
            {
                randXp1 = xAxis.Next(1, 4);
                randYp1 = yAxis.Next(1, 4);

            }

            if (p2Ammo <= 1 && p2AmmoBoard == 0)
            {
                randXp2 = xAxis.Next(9, 12);
                randYp2 = yAxis.Next(1, 4);
            }

            //Health
            if (p1Health <= 5 && p1HealthBoard == 0)
            {
                healthXp1 = xAxis.Next(1, 4);
                healthYp1 = yAxis.Next(1, 4);
            }

            if (p2Health <= 5 && p2HealthBoard == 0)
            {
                healthXp2 = xAxis.Next(9, 12);
                healthYp2 = yAxis.Next(1, 4);
            }

            //Pick up
            //ammo
            if (p1_x_pos == randXp1 && p1_y_pos == randYp1)
            {
                if (p1Ammo <= 1)
                {
                    p1Ammo++;
                    p1AmmoBoard--;
                }
                
            }

            if (p2_x_pos == randXp2 && p2_y_pos == randYp2)
            {
                if(p2Ammo <= 1)
                {
                    p2Ammo++;
                    p2AmmoBoard--;
                }
            }

            //Health
            if (p1_x_pos == healthXp1 && p1_y_pos == healthYp1)
            {
                if (p1Health <= 5)
                {
                    p1Health++;
                    p1HealthBoard--;
                }
            }

            if (p2_x_pos == healthXp2 && p2_y_pos == healthYp2)
            {
                if (p2Health <= 5)
                {
                    p2Health++;
                    p2HealthBoard--;
                }
            }

            //Shoot
            if (p1Shoot == true)
            {
                if(p1_y_pos == p2_y_pos)
                {
                    p2Health--;
                }
                
                p1Ammo--;
                p1Shoot = false;
            }
            if(p2Shoot == true)
            {
                if (p1_y_pos == p2_y_pos)
                {
                    p1Health--;
                }
                p2Ammo--;
                p2Shoot = false;
            }

            if (p1Health == 0 || p2Health == 0)
            {
                isPlaying = false;
            }
        }

        static void Draw()
        {
            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw player 1
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("O");

            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("O");

            //draw Ammo 
            if (p1Ammo <= 1)
            {
                if(p1AmmoBoard == 0)
                {
                    p1AmmoBoard++;
                }
                
                    Console.SetCursorPosition(randXp1, randYp1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("*");
            }

            if (p2Ammo <= 1)
            {
                if(p2AmmoBoard == 0)
                {
                    p2AmmoBoard++;
                }
                Console.SetCursorPosition(randXp2, randYp2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("*");
            }

            //draw health pickups
            if(p1Health < 5)
            {
                if(p1HealthBoard == 0)
                {
                    p1HealthBoard++;
                }

                Console.SetCursorPosition(healthXp1, healthYp1);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("+");
                Console.ResetColor();
            }

            if (p2Health < 5)
            {
                if (p2HealthBoard == 0)
                {
                    p2HealthBoard++;
                }

                Console.SetCursorPosition(healthXp2, healthYp2);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("+");
                Console.ResetColor();
            }

            // draw the Turn Indicator
            Console.SetCursorPosition(3, 5);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nUSE WASD or IJKL to move");
            Console.ForegroundColor = ConsoleColor.White;

            // display player health
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Player1 Health: " + p1Health);
            Console.WriteLine("Player1 Ammo: " + p1Ammo);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Player2 Health: " + p2Health);
            Console.WriteLine("Player2 Ammo: " + p2Ammo);

            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
