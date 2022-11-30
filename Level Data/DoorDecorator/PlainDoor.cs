using Newtonsoft.Json.Linq;

namespace Level_Data.StrategyPattern
{

        public class PlainDoor : Door
        {
            public PlainDoor()
            {
            }

            public bool Open(Game game)
            {
                return true;
            }
        }
    
}

