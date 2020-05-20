using Domain.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.IO
{
    public static class FileProcessor
    {
        private const string TurtleRegex = @"(\d*[,]\d*)( facing )(\w*)";
        private const string ExitRegex = @"(\d*[,]\d*)";
        private const string MinesRegex = @"(\d*[,]\d*)";
        private const string MovesRegex = @"(- )([\w\,]*)";
        private const string GridSizeRegex = @"(\d*[,]\d*)";

        public static Sprite InitializeTurtle(string[] args)
        {
            string firstLine = ReadFile(args[0], 0);

            var matches = Regex.Match(firstLine, TurtleRegex);

            string[] coordinates = matches.Groups[1].Value.Split(',');
            string direction = matches.Groups[3].Value.ToLower();

            return new Sprite
                (
                    posX: int.Parse(coordinates[0]),
                    posY: int.Parse(coordinates[1]),
                    body: "T",
                    direction: direction
                );
        }

        public static Sprite InitializeExit(string[] args)
        {
            string secondLine = ReadFile(args[0], 1);

            var match = Regex.Match(secondLine, ExitRegex);

            string[] coordinates = match.Value.Split(',');

            return new Sprite
                (
                    posX: int.Parse(coordinates[0]),
                    posY: int.Parse(coordinates[1]),
                    body: "E"
                );
        }

        public static List<Sprite> InitializeMines(string[] args)
        {
            string thirdLine = ReadFile(args[0], 2);

            List<Sprite> mines = new List<Sprite>();

            var matches = Regex.Matches(thirdLine, MinesRegex);

            foreach (var match in matches)
            {
                var coordinate = match.ToString().Split(',');
                mines.Add(new Sprite(int.Parse(coordinate[0]), int.Parse(coordinate[1]), "M"));
            }

            return mines;
        }

        public static List<string> InitializeMoves(string[] args)
        {
            string firstLine = ReadFile(args[1], 0);

            var matches = Regex.Match(firstLine, MovesRegex);

            string[] moves = matches.Groups[2].Value.Split(',');

            List<string> listOfMoves = new List<string>();

            foreach (var move in moves)
            {
                listOfMoves.Add(move);
            }
            return listOfMoves;
        }

        public static Grid InitializeGrid(string[] args)
        {
            string fourthLine = ReadFile(args[0], 3);

            var size = Regex.Match(fourthLine, GridSizeRegex);

            string[] coordinate = size.Value.Split(',');

            return new Grid(int.Parse(coordinate[0]), int.Parse(coordinate[1]));
        }

        private static string ReadFile(string fileName, int lineNmber)
        {
            var fullPath = fileName.FullFilePath();

            if (File.Exists(fullPath))
            {
                Logger.Log($"File found - {fullPath}", ConsoleColor.Green);
                try
                {
                    string[] lines = File.ReadAllLines(fullPath);
                    return lines[lineNmber];
                }
                catch(Exception e)
                {
                    Logger.Log($"Error while reading {fileName}", ConsoleColor.Red);
                    Logger.Log($"{e.StackTrace}", ConsoleColor.Red);
                }                
            }
            Logger.Log($"{fileName} not found, please ensure it exists in current directory", ConsoleColor.Red);
            return null;
        }

        private static string FullFilePath(this string filename)
        {
            return Directory.GetCurrentDirectory() + $"\\{filename}";
        }
    }
}
