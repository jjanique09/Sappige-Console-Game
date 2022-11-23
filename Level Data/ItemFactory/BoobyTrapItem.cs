
namespace Level_Data.StrategyPattern
{

        public class BoobyTrapItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public BoobyTrapItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.lives -= damage.Value;
            }
        }
    
}

