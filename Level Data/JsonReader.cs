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

            PlainDoor plainPizzaObj = new PlainDoor();
            DoorDecorator chickenPizzaDecorator = new ColoredDoorDecorator(plainPizzaObj);
            Door vegPizzaDecorator = new VegPizzaDecorator(chickenPizzaDecorator);

            Console.WriteLine("1 "+ chickenPizzaDecorator.Open());
            Console.WriteLine("1 "+ vegPizzaDecorator.Open());

            Console.Read();

            /*

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
            */


            var list = data as List<string>;
            list.Sort();
            return list;
        }

        public interface Door
        {
            bool Open(Game game);
        }

        
        public class PlainDoor : Door
        {
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
            public ColoredDoorDecorator(Door door) : base(door)
            {
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) == true)
                {
                    if (game.player.items.FindAll(x=>x.type.Equals("key") && x.color.Equals("red")).Count() > 0 ) 
                    {
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
        }

        // Stap 5 Creating Veg Pizza Decorator

        public class VegPizzaDecorator : DoorDecorator
        {
            public VegPizzaDecorator(Door door) : base(door)
            {
            }
            public override bool Open(Game game)
            {
                if (door.Open(game) == true)
                {
                    if (player == null)
                    {
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
        }







        // Je wil hebt een methode die bepaald of een deur open mag voor elk verschillend deur type - Fixed
        // Deze methode geeft een booelean terug maar hoe decorate je dan 2 verschillende deuren?
        // Er moeten parameters worden doorgevoerd om bepaalde variabelen te setten zoals: color, no_of_stones. het variabele "Type" word bepaald door de decorator
        // 








        /*
                public class Door
                {
                    public string? type { get; set; }
                    public string? color { get; set; }
                    public int? no_of_stones { get; set; }

                }


        */

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


        private List<Item> CreateRoomItems(JToken items) /// Convert to Factory -> That also adds pickup/step-on logic.
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
                if (connection["doors"] != null) { setConnection.doors = CreateDoors(JToken.FromObject(connection["doors"])); }
                connections.Add(setConnection);
            }
            return connections;
        }


        private Door CreateDoors(JToken items)
        {
            Door itemList = new Door();
            foreach (var jsonItem in items)
            {
                var type = jsonItem["type"].Value<string>();
                Door a = new Door();
            }
            return itemList;
        }







        /// <summary>
        /// 
        /// These Classes are currently internal but have to become a external.
        /// The classes are in this class while the development of the decorators is 
        /// because this allows me to develop faster without having to jump around classes
        /// 
        /// </summary>

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
            public List<Door>? doors { get; set; }
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

