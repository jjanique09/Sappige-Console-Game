
namespace Level_Data.StrategyPattern
{

        public class SankaraStoneItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public SankaraStoneItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.items.Add(new SankaraStoneItem(type, color, x, y, damage));
            }
    }
}

