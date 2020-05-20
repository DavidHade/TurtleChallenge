using Domain.Models;
using Domain.Helpers;
using Domain.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Settings;
using System.Threading;

namespace Domain.Engine
{
    public interface IGameEngine
    {
        public GameStatus Start(SettingsInitializer settngs);
        public GameStatus Move(Grid grid, Sprite sprite, string action, List<KeyValuePair<string, ConsoleColor>> outputs);
        public GameStatus CheckCollision(Grid grid, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs);
    }

    public class GameEngine : IGameEngine
    {
        IDrawingEngine _drawingEngine;
        GameStatus _status;

        public GameEngine(IDrawingEngine engine, GameStatus status)
        {
            _drawingEngine = engine;
            _status = status;
        }

        public GameStatus Start(SettingsInitializer settings)
        {
            settings.DrawEngine.DrawGrid(settings.Grid, settings.Outputs);
            Thread.Sleep(settings.GameSpeed);

            int moves = settings.Moves.Count();
            int completed = 0;

            foreach (var move in settings.Moves)
            {
                _status = Move(settings.Grid, settings.Turtle, move, settings.Outputs);

                if (_status == GameStatus.HitTheWall || _status == GameStatus.ReachedExit)
                {
                    break;
                }
                Thread.Sleep(settings.GameSpeed);
                completed ++;
            }
            return moves == completed ? GameStatus.OutOfMoves : _status;
        }

        public GameStatus Move(Grid grid, Sprite sprite, string action, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            if (action == "r")
            {
                sprite.Direction = ChangeDirection(sprite, outputs);
                return GameStatus.ChangedDirection;
            }
            if (action != "m" && action != "r")
            {
                outputs.Add(new KeyValuePair<string, ConsoleColor>("Invalid command given, skipping step", ConsoleColor.Blue));
                return GameStatus.SkippingStep;
            }

            var previousX = sprite.PositionX;
            var previousY = sprite.PositionY;

            switch (sprite.Direction)
            {
                case "north":
                    sprite.PositionY -= 1;
                    break;
                case "south":
                    sprite.PositionY += 1;
                    break;
                case "east":
                    sprite.PositionX -= 1;
                    break;
                case "west":
                    sprite.PositionX += 1;
                    break;
            }

            _status = CheckCollision(grid, sprite, outputs);
            _drawingEngine.Update(grid, previousX, previousY, sprite, outputs);
            _drawingEngine.DrawGrid(grid, outputs);

            return _status;
        }

        public GameStatus CheckCollision(Grid grid, Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            if (!IsWithinBounds(grid, sprite))
            {
                outputs.Add(new KeyValuePair<string, ConsoleColor>("The turtle has hit a wall!", ConsoleColor.Red));
                return GameStatus.HitTheWall;
            }

            var currentTile = grid.Tiles.GetValue(sprite.PositionX, sprite.PositionY)?.ToString();

            if (!string.IsNullOrEmpty(currentTile) && currentTile.Contains(Tiles.Mine))
            {
                outputs.Add(new KeyValuePair<string, ConsoleColor>("The turtle has encountered a mine!", ConsoleColor.Red));
                return GameStatus.SteppedOnAMine;
            }

            if (!string.IsNullOrEmpty(currentTile) && currentTile.Contains(Tiles.Exit))
            {
                outputs.Add(new KeyValuePair<string, ConsoleColor>("The turtle has safely escaped to the exit.", ConsoleColor.Green));
                return GameStatus.ReachedExit;
            }

            outputs.Add(new KeyValuePair<string, ConsoleColor>("Successful move performed", ConsoleColor.Green));
            return GameStatus.SuccessfulMove;
        }

        private bool IsWithinBounds(Grid grid, Sprite sprite)
        {
            var lenX = grid.Tiles.Length / grid.Tiles.GetLength(1);
            var lenY = grid.Tiles.GetLength(1);

            return sprite.PositionX >= 0 && sprite.PositionY >= 0 && sprite.PositionX < lenX && sprite.PositionY < lenY;
        }

        private string ChangeDirection(Sprite sprite, List<KeyValuePair<string, ConsoleColor>> outputs)
        {
            var currentDirection = Direction.Directions.IndexOf(sprite.Direction);

            outputs.Add(new KeyValuePair<string, ConsoleColor>("Changed sprite direction", ConsoleColor.Green));

            return sprite.Direction == Direction.Directions.Last() ?
                sprite.Direction = Direction.Directions.First() : 
                sprite.Direction = Direction.Directions[currentDirection + 1];
        }

    }
}
