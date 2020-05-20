
namespace Domain.Models
{
    public class Sprite
    {
        public int PositionX;
        public int PositionY;
        public string Character;
        public string Direction;

        public Sprite(int posX, int posY, string body, string direction = null)
        {
            PositionX = posX;
            PositionY = posY;
            Character = body;
            Direction = direction;
        }
    }
}
