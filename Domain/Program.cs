using Domain.Settings;
using System;
using System.Diagnostics;

namespace Domain
{
    class Program
    {
        static void Main(string[] args)
        {            
            var settings = new SettingsInitializer();

            if (Debugger.IsAttached)
            {
                settings.Debug = true;
            }
            
            settings.InitializeSettings(args);

            var engine = settings.GameEngine;
            var status = engine.Start(settings);
            
            Console.WriteLine($"Game over - {status.ToString()}");
            Console.ReadLine();
        }
    }
}
