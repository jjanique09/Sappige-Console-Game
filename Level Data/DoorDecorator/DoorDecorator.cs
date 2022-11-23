
namespace Level_Data.StrategyPattern
{

        public abstract class DoorDecorator : Door
        {
            protected Door door;

            public DoorDecorator(Door door)
            {
                this.door = door;
            }

            public virtual bool Open(Game game)
            {
                return door.Open(game);
            }
        }
    
}

