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
                if (toggle == true)
                {
                    toggle = false;
                    return true;
                }
                else
                {
                    toggle = true;
                    return false;
                }
            }
        }
}

