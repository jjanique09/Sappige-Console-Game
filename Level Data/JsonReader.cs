using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using static Level_Data.JsonReader;

namespace Level_Data
{
    class JsonReader : IStrategy
    {

        JObject GameJsonObj = JObject.Parse(File.ReadAllText(@"../LevelDataJson.json"));
        public object DoAlgorithm(object data)
        {
            List<Connection> connections = CreateConnection(GameJsonObj);
            var list = data as List<string>;
            list.Sort();                                                                                                                                                                                                                                                                                                                                                                                                                 
            return list;
        }


        public void PrintLoadedGameData()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPrinted Player\n" + " Lives = " + CreatePlayer().lives + "\n StartX = " + CreatePlayer().startX + "\n StartY = " + CreatePlayer().startY + "\n StartRoomId = " + CreatePlayer().startRoomId);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nPrinting All Rooms");
            foreach (var room in CreateRoom())
            {
                Console.WriteLine(" RoomID = " + room.id + "\n Height = " + room.height + "\n Width = " + room.width + "\n Type = " + room.type + "\n");
                foreach (var roomItem in room.items)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("  Item Type = " + roomItem.type + "\n" + "  Color = " + roomItem.color + "\n" + "  X = " + roomItem.x + "\n" + "  Y = " + roomItem.y + "\n" + "  Damage = " + roomItem.damage + "\n");
                }
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.ForegroundColor = ConsoleColor.White;

            
        }


        public interface Door
        {
            bool Open(Game game);
        }


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


        public class ColoredDoorDecorator : DoorDecorator
        {
            public ColoredDoorDecorator(Door door,string color) : base(door) 
            {
            }
            public override bool Open(Game game)
            {
                    if (door.Open(game) == true && game.player.items.FindAll(x => x.type.Equals("key") && x.color.Equals("red")).Count() > 0)
                    {
                        return true;
                    }
                    else 
                    { 
                        return false; 
                    }
            }
        }


        public class OpenOnOddDoorDecorator : DoorDecorator
        {
            public OpenOnOddDoorDecorator(Door door) : base(door)
            {
            }
            public override bool Open(Game game)
            {
                    if (door.Open(game) == true && game.player.items.Where(x=>x.type.Equals("sankara stone")).Count()%2 != 0)
                    {
                        return true;
                    }
                    else 
                    { 
                        return false; 
                    }
            }
        }


        public class OpenOnStonesInRoomDoorDecorator : DoorDecorator
        {
            public int numberOfStones { get; set; }
            public OpenOnStonesInRoomDoorDecorator(Door door,int setNumberOfStones) : base(door)
            {
                numberOfStones = setNumberOfStones;
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) == true && game.rooms[game.player.startRoomId].items.Where(x => x.type.Equals("sankara stone")).Count() == numberOfStones)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class ToggleDoorDecorator : DoorDecorator
        {
            public bool toggle { get; set; }
            public ToggleDoorDecorator(Door door) : base(door)
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

        public Player CreatePlayer()
        {
            JToken player = GameJsonObj["player"];
            return new Player
            {
                startY = player["startY"].Value<int>(),
                startX = player["startX"].Value<int>(),
                startRoomId = player["startRoomId"].Value<int>(),
                lives = player["lives"].Value<int>(),
            };
        }


        public List<Room> CreateRoom()
        {
            List<Room> roomList = new List<Room>();
            foreach (var room in GameJsonObj["rooms"])
            {
                List<Item> setItems = new List<Item>();
                if (room["items"] != null) { setItems = CreateRoomItems(JToken.FromObject(room["items"])); }
                roomList.Add(new Room
                {
                    id = room["id"].Value<int>(),
                    width = room["width"].Value<int>(),
                    height = room["height"].Value<int>(),
                    type = room["type"].Value<string>(),
                    items = setItems
                });
            }
            return roomList;
        }


        private List<Item> CreateRoomItems(JToken items) 
        {
            List<Item> itemList = new List<Item>();
            foreach (var jsonItem in items)
            {
                itemList.Add(new Item
                {
                    type = jsonItem["type"].Value<string>(),
                    color = (jsonItem["color"] ?? "").Value<string>(),
                    x = jsonItem["x"].Value<int>(),
                    y = jsonItem["y"].Value<int>(),
                    damage = (jsonItem["damage"] ?? 0).Value<int>()
                });
            }
            return itemList;
        }


        public List<Connection> CreateConnection(JObject json)
        {
            var connections = new List<Connection>();
            foreach (var connection in json["connections"])
            {
                Connection setConnection = new Connection();
                if (connection["NORTH"] != null) { setConnection.north = connection["NORTH"].Value<int?>(); }
                if (connection["EAST"] != null) { setConnection.east = connection["EAST"].Value<int?>(); }
                if (connection["WEST"] != null) { setConnection.west = connection["WEST"].Value<int?>(); }
                if (connection["SOUTH"] != null) { setConnection.south = connection["SOUTH"].Value<int?>(); }

                List<Door> doors = new List<Door>();
                if (connection["doors"] != null) { setConnection.door = CreateDoors(JToken.FromObject(connection["doors"])); }
                connections.Add(setConnection);
            }
            return connections;
        }


        private Door CreateDoors(JToken doorToken)
        {
            bool firstInitilizer = true;
            DoorDecorator? doorDecorator = null;
            Console.WriteLine("");
            foreach (var jsonItem in doorToken)
            {

                if (firstInitilizer == true)
                {
                    PlainDoor doorObj = new PlainDoor(doorToken);
                    string type = jsonItem["type"].Value<string>();
                     
                    if (type.Equals("colored")) { doorDecorator = new ColoredDoorDecorator(doorObj, jsonItem["color"].Value<string>()); Console.Write("You Decorated Colors with"); }
                    if (type.Equals("open on odd")) { doorDecorator = new OpenOnOddDoorDecorator(doorObj); Console.Write("You Decorated Open On Odd with"); }

                    //Uncomment check if continuity is still correct ---> // Console.WriteLine("Dit moet een 1 zijn verdikkie " + doorToken.Count());
                    //Console.WriteLine(doorToken.Count());
                    firstInitilizer = false;
                    if (doorToken.Count() == 1) { return doorDecorator;} 
                }
                else
                {
                    //Uncomment check if continuity is still correct ---> // Console.WriteLine("Dit mag geen 1 zijn verdikkie " + doorToken.Count());
                    Door? xDecorator = null;
                    string type = jsonItem["type"].Value<string>();
                    if (type.Equals("colored")) { xDecorator = new ColoredDoorDecorator(doorDecorator, jsonItem["color"].Value<string>()); Console.Write("Colored"); }
                    if (type.Equals("open on odd")) { xDecorator = new OpenOnOddDoorDecorator(doorDecorator); Console.Write(" Open On Odd"); } 
                    
                    return xDecorator;
                }
            }
            return doorDecorator;
        }
           
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

        public class Connection
        {
            public int? north { get; set; }
            public int? east { get; set; }
            public int? south { get; set; }
            public int? west { get; set; }
            public Door door { get; set; }
        }


        public class Item 
        {
           public string type { get; set; }
           public string? color { get; set; }
           public int x { get; set; }
           public int y { get; set; }
           public int? damage { get; set; }
        }


        public class Room
        {
            public int id { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string type { get; set; }
            public List<Item> items { get; set; }
        }


        public class Player
        {
            public int startRoomId { get; set; }
            public int startX { get; set; }
            public int startY { get; set; }
            public int lives { get; set; }
            public List<Item> items { get; set; }
        }
    }
}

