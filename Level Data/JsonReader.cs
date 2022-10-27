using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Level_Data.JsonReader;

namespace Level_Data
{
    class JsonReader : IStrategy
    {
        JObject GameJsonObj = JObject.Parse(File.ReadAllText(@"../LevelDataJson.json"));

        public object DoAlgorithm(object data)
        {

            var regularOrder = new RegularDoor();
            Console.WriteLine(regularOrder.doorOpens());
            Console.WriteLine();
            var preOrder = new ToggleDoor();
            Console.WriteLine(preOrder.doorOpens());
            Console.WriteLine();
            var premiumPreorder = new PremiumPreorder(preOrder);
            Console.WriteLine(premiumPreorder.doorOpens());

            /*
             //code-maze.com/decorator-design-pattern

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
                if (connection["doors"] != null) { setConnection.doors = CreateDoors(JToken.FromObject(connection["doors"])); }
                connections.Add(setConnection);
            }
            return connections;
        }

        private List<Door> CreateDoors(JToken items)
        {
            List<Door> itemList = new List<Door>();
            foreach (var jsonItem in items)
            {
                var type = jsonItem["type"].Value<string>();
                
                
                
                
                
                Door a = new Door();

                itemList.Add(a);
            }
            return itemList;
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
        }
	}
}


public class Door
{
    public string? type { get; set; }
    public string? color { get; set; }
    public int? no_of_stones { get; set; }

}
