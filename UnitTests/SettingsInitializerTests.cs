using Domain.Settings;
using NUnit.Framework;

namespace UnitTests
{
    public class SettingsInitializerTests
    {
        SettingsInitializer _settings;

        [SetUp]
        public void SetUp()
        {
            _settings = new SettingsInitializer();
        }

        [Test]
        public void DebugEnvironmentSettingsAreCorrect()
        {
            _settings.Debug = true;
            _settings.InitializeSettings(new string[2]);

            Assert.That(_settings.DrawEngine, Is.Not.Null);
            Assert.That(_settings.GameEngine, Is.Not.Null);

            Assert.That(_settings.Turtle.Character, Is.Not.Null);
            Assert.That(_settings.Turtle.PositionX, Is.Not.Null);
            Assert.That(_settings.Turtle.PositionY, Is.Not.Null);
            Assert.That(_settings.Turtle.Direction, Is.Not.Null);

            Assert.That(_settings.Exit.Character, Is.Not.Null);
            Assert.That(_settings.Exit.PositionX, Is.Not.Null);
            Assert.That(_settings.Exit.PositionY, Is.Not.Null);

            Assert.That(_settings.Grid.Tiles.Length > 0);
            Assert.That(_settings.Mines.Count > 0);
        }
    }
}
