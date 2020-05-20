using Domain.Engine;
using Domain.Models;
using Domain.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTests
{
    public class GameEngineTests
    {
        SettingsInitializer _settings;
        IGameEngine _engine;

        [SetUp]
        public void SetUp()
        {
            _settings = new SettingsInitializer();
            _settings.Debug = true;
            _settings.GameSpeed = 1;

            InitializeGame();
        }

        [Test]
        public void SimulateNumberOfRounds()
        {
            int rounds = 100;            

            for (int i = 0; i < rounds; i++)
            {
                var result = _engine.Start(_settings);

                Debug.WriteLine($"Run #{i} finished with {result}");

                Assert.That(result == GameStatus.HitTheWall ||
                            result == GameStatus.OutOfMoves ||
                            result == GameStatus.ReachedExit);

                InitializeGame();
            } 
        }

        [Test]
        public void MovingTests()
        {
            var outputs = new List<KeyValuePair<string, ConsoleColor>>();
            var grid = new Grid(10, 10);
            var turtle = new Sprite(5, 7, Tiles.Turtle);

            var status = _engine.Move(grid, turtle, "r", outputs);
            Assert.That(status == GameStatus.ChangedDirection);

            status = _engine.Move(grid, turtle, "m", outputs);
            Assert.That(status == GameStatus.SuccessfulMove);

            status = _engine.Move(grid, turtle, "abc", outputs);
            Assert.That(status == GameStatus.SkippingStep);
        }

        [Test]
        public void CollisionTest()
        {
            var outputs = new List<KeyValuePair<string, ConsoleColor>>();
            var grid = new Grid(10, 10);
            var turtle = new Sprite(10, 10, Tiles.Turtle);

            var status = _engine.CheckCollision(grid, turtle, outputs);

            Assert.That(status == GameStatus.HitTheWall);
        }

        [Test]
        public void MineCollisionTest()
        {
            var outputs = new List<KeyValuePair<string, ConsoleColor>>();
            var grid = new Grid(10, 10);
            grid.Tiles.SetValue(Tiles.Mine, 3, 4);
            var turtle = new Sprite(3, 4, Tiles.Turtle);

            var status = _engine.CheckCollision(grid, turtle, outputs);

            Assert.That(status == GameStatus.SteppedOnAMine);
        }

        [Test]
        public void ExitCollisionTest()
        {
            var outputs = new List<KeyValuePair<string, ConsoleColor>>();
            var grid = new Grid(10, 10);
            grid.Tiles.SetValue(Tiles.Exit, 7, 9);
            var turtle = new Sprite(7, 9, Tiles.Turtle);

            var status = _engine.CheckCollision(grid, turtle, outputs);

            Assert.That(status == GameStatus.ReachedExit);
        }

        private void InitializeGame()
        {
            _settings.InitializeSettings(new string[2]);
            _engine = _settings.GameEngine;
        }

    }
}
