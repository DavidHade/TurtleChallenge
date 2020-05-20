using System;
using Domain.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Domain.IO;

namespace Domain.Drawing
{
    public interface IDrawingEngine
    {
        void DrawGrid(Grid grid, List<KeyValuePair<string, ConsoleColor>> outputs);
        void Add(Grid grid, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs);
        void Update(Grid grid, int currentX, int currentY, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs);
    }

    public class DrawingEngine : IDrawingEngine
    {
        public void DrawGrid(Grid grid, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            //Unit tests will throw an IO Exception when setting cursor position
            try { Console.SetCursorPosition(0, 0); }
            catch { }

            var lenX = grid.Tiles.Length / grid.Tiles.GetLength(1);
            var lenY = grid.Tiles.GetLength(1);

            DrawBorder(lenX);

            for (int y = 0; y < lenY; y++)
            {
                Console.Write(Tiles.TileStart);

                for (int x = 0; x < lenX; x++)
                {
                    if ((grid.Tiles.GetValue(x, y) != null) &&
                        Regex.IsMatch(grid.Tiles.GetValue(x, y)?.ToString(), "\\w"))
                    {

                        Console.Write(grid.Tiles.GetValue(x, y));
                    }
                    else
                    {
                        Console.Write(Tiles.Tile);
                    }
                }

                Console.WriteLine("\r");
                DrawBorder(lenX);                
            }
            Logger.Log(outputs);
        }

        public void Add(Grid grid, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            try
            {
                grid.Tiles.SetValue($" {sprite.Character} |", sprite.PositionX, sprite.PositionY);
            }
            catch(IndexOutOfRangeException)
            {
                outputs.Add(new KeyValuePair<string, ConsoleColor>("Specified coordinates are outside the bounds of the array", ConsoleColor.White));
            }            
        }

        public void Update(Grid grid, int currentX, int currentY, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            try
            {
                grid.Tiles.SetValue(Tiles.Tile, currentX, currentY);
                grid.Tiles.SetValue($" {sprite.Character} |", sprite.PositionX, sprite.PositionY);
            }
            catch(IndexOutOfRangeException)
            {
                grid.Tiles.SetValue($" {sprite.Character} |", currentX, currentY);
            }     
        }

        private static void DrawBorder(int n)
        {            
            for (int m = 0; m <= n - 1; m++)
            {
                Console.Write(Tiles.Border);
            }
            Console.WriteLine("\r");
        }
    }
}
