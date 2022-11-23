

namespace Level_Data.StrategyPattern
{

            public class Game
            {
                public Game(Player setPlayer, List<Connection> setConnections, List<Room> setRooms)
                {
                    player = setPlayer;
                    connections = setConnections;
                    rooms = setRooms;
                }

                public Player player { get; set; }
                public List<Connection> connections { get; set; }
                public List<Room> rooms { get; set; }
            }
        
    }


