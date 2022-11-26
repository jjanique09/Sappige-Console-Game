
namespace Level_Data.StrategyPattern
{

        public class OpenOnOddDoorDecorator : DoorDecorator
        {
            public OpenOnOddDoorDecorator(Door door) : base(door)
            {
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) && game.player.items.Where(x => x.type.Equals("sankara stone")).Count() % 2 != 0)
                {
                    return true;
                }
             return false;
                
            }
        }
    
}

