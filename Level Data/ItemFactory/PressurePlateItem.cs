
namespace Level_Data.StrategyPattern
{
        public class PressurePlateItem : Iitem 
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public PressurePlateItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {

                if (game.rooms.Where(x =>x.id == game.player.startRoomId).First().toggle == true)
                {
                    game.rooms.Where(x => x.id == game.player.startRoomId).First().toggle = false;
                }
                
                else if (game.rooms.Where(x => x.id == game.player.startRoomId).First().toggle == false) 
                { 
                    game.rooms.Where(x => x.id == game.player.startRoomId).First().toggle = true;
                }
            }
        }
    }

