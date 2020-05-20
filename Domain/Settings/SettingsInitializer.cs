using Domain.Engine;
using Domain.Drawing;
using Domain.Models;
using System;
using System.Collections.Generic;
using Domain.Helpers;
using Domain.IO;

namespace Domain.Settings
{
    public interface ISettingsInitializer
    {
        void InitializeSettings(string[] args);
    }

    public class SettingsInitializer : ISettingsInitializer
    {
        public Sprite Turtle;
        public Sprite Exit;
        public List<Sprite> Mines;
        public Grid Grid;
        public List<string> Moves;
        public List<KeyValuePair<string, ConsoleColor>> Outputs;
        public GameEngine GameEngine;
        public DrawingEngine DrawEngine;        
        public int GridSize;
        public int GridXBound;
        public int GridYBound;
        public GameStatus status;
        public int GameSpeed = 1500;
        public bool Debug = false;

        public void InitializeSettings(string[] args)
        {
            // If enabled this will generate random inputs
            if (Debug)
            {                
                InitializeOutputs();
                InitializeEngine();
                InitializeMap();
                InitializeSprites();
                InitializeMoves();
            }
            else
            {
                InitializeOutputs();
                InitializeEngine();
                Turtle = FileProcessor.InitializeTurtle(args);
                Exit = FileProcessor.InitializeExit(args);
                Mines = FileProcessor.InitializeMines(args);
                Grid = FileProcessor.InitializeGrid(args);
                Moves = FileProcessor.InitializeMoves(args);
                InitializeConsole();
                DrawSprites();                
            } 
        }

        private void InitializeConsole()
        {
            Console.Clear();            
        }

        private void InitializeMap()
        {
            Grid = new Grid(Random(10, 15), Random(10, 15));

            GridXBound = Grid.Tiles.Length / Grid.Tiles.GetLength(1) - 1;
            GridYBound = Grid.Tiles.GetLength(1) - 1;
            GridSize = GridXBound * GridYBound;
        }

        private void InitializeEngine()
        {
            DrawEngine = new DrawingEngine();
            GameEngine = new GameEngine(DrawEngine, new GameStatus());
        }

        private void DrawSprites()
        {
            foreach (var mine in Mines)
            {
                DrawEngine.Add(Grid, mine, Outputs);
            }            
            DrawEngine.Add(Grid, Exit, Outputs);
            DrawEngine.Add(Grid, Turtle, Outputs);
        }

        private void InitializeSprites()
        {
            Turtle = new Sprite(RandomX(), RandomY(), Tiles.Turtle, Direction.Directions[Random(0,3)]);
            Exit = new Sprite(RandomX(), RandomY(), Tiles.Exit);
            Mines = new List<Sprite>();

            int mineCount = GridSize / 15;

            for (int i = 0; i < mineCount; i++)
            {
                Mines.Add(new Sprite(RandomX(), RandomY(), Tiles.Mine));
            }            
            foreach (var mine in Mines)
            {
                DrawEngine.Add(Grid, mine, Outputs);
            }
            DrawEngine.Add(Grid, Turtle, Outputs);
            DrawEngine.Add(Grid, Exit, Outputs);
        }

        private void InitializeMoves()
        {
            Moves = new List<string>();

            for (int i = 0; i < GridSize / 10; i++)
            {
                if (i % 3 == 0)
                {
                    Moves.Add("r");
                }
                else
                {
                    Moves.Add("m");
                }
            }
        }

        private void InitializeOutputs()
        {
            Outputs = new List<KeyValuePair<string, ConsoleColor>>();
            Outputs.Add(new KeyValuePair<string, ConsoleColor>("The game has started!", ConsoleColor.Yellow));
        }

        #region Pseudo-random numbers generation
        private int Random(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private int RandomX()
        {
            Random random = new Random();
            return random.Next(0, GridXBound);
        }
        private int RandomY()
        {
            Random random = new Random();
            return random.Next(0, GridYBound);
        }
        #endregion
    }
}
