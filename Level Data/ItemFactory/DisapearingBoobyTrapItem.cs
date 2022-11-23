
namespace Level_Data.StrategyPattern
{

        public class DisapearingBoobyTrapItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public DisapearingBoobyTrapItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.rooms.Where(x => x.id == game.player.startRoomId).First().items.RemoveAll(e => e.type.Equals("disappearing boobytrap") && e.x == x && e.y == y);
                game.player.lives -= damage.Value;
            }
        }
    }


