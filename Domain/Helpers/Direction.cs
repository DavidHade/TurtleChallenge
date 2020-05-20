using System.Collections.Generic;

namespace Domain.Helpers
{
    public static class Direction
    {
        public static IList<string> Directions = new List<string> 
        { 
            "north",
            "east",
            "south",
            "west"
        };

        public static string North { get { return "north"; } }
        public static string East { get { return "east"; } }
        public static string South { get { return "south"; } }
        public static string West { get { return "west"; } }
        
    }
}
