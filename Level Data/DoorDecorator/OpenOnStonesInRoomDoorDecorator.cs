
namespace Level_Data.StrategyPattern
{

        public class OpenOnStonesInRoomDoorDecorator : DoorDecorator
        {
            public int numberOfStones { get; set; }
            public OpenOnStonesInRoomDoorDecorator(Door door, int setNumberOfStones) : base(door)
            {
                numberOfStones = setNumberOfStones;
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) && game.rooms[game.player.startRoomId].items.Where(x => x.type.Equals("sankara stone")).Count() == numberOfStones)
                {
                    return true;
                }
                return false;
                
            }
        
    }
}

