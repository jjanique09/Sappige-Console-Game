using static Level_Data.StrategyPattern.JsonReader;

namespace Level_Data.StrategyPattern
{
 
        public class ColoredDoorDecorator : DoorDecorator
        {
            public ColoredDoorDecorator(Door door, string color) : base(door)
            {
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) && game.player.items.FindAll(x => x.type.Equals("key") && x.color.Equals("red")).Count() > 0)
                {
                    return true;
                }
                
            return false;
                
            }
        }
    }


