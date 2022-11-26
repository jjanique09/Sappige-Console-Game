namespace Level_Data.StrategyPattern
{

        public class ClosingDoorDecorator : DoorDecorator
        {
            public bool toggle { get; set; }
            public ClosingDoorDecorator(Door door) : base(door)
            {
                toggle = true;
            }
            public override bool Open(Game game)
            {
                   toggle = !toggle;
                   return !toggle;
            }
        }
}

