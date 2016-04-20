﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolePlayArea;
using B1tClass;
using System.Threading;

namespace B1trisV2
{
    class B1trisv2
    {


        static void Main(string[] args)
        {
            //settings
            Console.Clear();

            PlayArea playField = new PlayArea();

            Console.BufferWidth = Console.WindowWidth = playField.CanvasWidth;
            Console.BufferHeight = Console.WindowHeight = playField.CanvasHeight;

            Console.Title = "B1tTris: The Game";

            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;

            Console.Clear();

            int difficultyMin = 1;
            int difficultyMax = 31;

            int appSpeed = 300; // lower is faster
            bool appIsRunning = true;

            Random mainRandomizer = new Random();
            //end settings

            //variables
            // empty playing field 
            List<string> existingRows = new List<string>();
            for (int row = 0; row < playField.Height; row++)
            {
                string toAdd = "";
                toAdd = toAdd.PadLeft(playField.Width, ' ');
                existingRows.Add(toAdd);
            }
            //
            Player player1 = new Player();

            while (appIsRunning)
            {
                //create a new piece
                player1.B1t = new B1ts(mainRandomizer.Next(difficultyMin, difficultyMax));
                player1.B1t.PosX = (playField.Width - player1.B1t.toPrint.Length) / 2;

                while(player1.B1t.isMoving)
                {
                    //read Input
                    while (Console.KeyAvailable)
                    {
                        player1.Input = Console.ReadKey();
                        switch (player1.Input.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                player1.B1t.hSpeed += -1;
                                if (player1.B1t.hSpeed < -3)
                                {
                                    player1.B1t.hSpeed = -3;
                                }
                                player1.Input = new ConsoleKeyInfo();
                                break;
                            case ConsoleKey.RightArrow:
                                player1.B1t.hSpeed += 1;
                                if (player1.B1t.hSpeed > 3)
                                {
                                    player1.B1t.hSpeed = 3;
                                }
                                player1.Input = new ConsoleKeyInfo();
                                break;
                        }
                    }
                    //Move Left or Right 
                    if ( player1.B1t.hSpeed> 0)
                    {
                        player1.B1t
                    }


                    Thread.Sleep(appSpeed);
                }

            }
        }
    }
}
