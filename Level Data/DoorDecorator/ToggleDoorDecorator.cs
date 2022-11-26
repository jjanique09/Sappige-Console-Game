
namespace Level_Data.StrategyPattern
{

        public class ToggleDoorDecorator : DoorDecorator
        {
            public ToggleDoorDecorator(Door door) : base(door)
            {
            }
            
            public override bool Open(Game game)
            {
                if (game.rooms.Where(x => x.id == game.player.startRoomId).First().toggle)
                {
                    return true;
                }
             return false;
             
            }
        
    }
}

