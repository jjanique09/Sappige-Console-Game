using Newtonsoft.Json.Linq;

namespace Level_Data.StrategyPattern
{

        public class PlainDoor : Door
        {
            public JToken DoorToken { get; set; }

            public PlainDoor(JToken doorToken)
            {
                DoorToken = doorToken;
            }

            public bool Open(Game game)
            {
                return true;
            }
        }
    
}

