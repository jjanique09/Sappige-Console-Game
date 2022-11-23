
namespace Level_Data.StrategyPattern
{

        public interface Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public void Use(Game game);
        }
    }


