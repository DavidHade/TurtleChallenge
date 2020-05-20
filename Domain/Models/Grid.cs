
namespace Domain.Models
{
    public class Grid
    {
        public string[,] Tiles;

        public Grid(int x, int y)
        {
            Tiles = new string[x, y];
        }
    }
}
