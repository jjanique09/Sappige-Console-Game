using Newtonsoft.Json.Linq;

namespace Level_Data.StrategyPattern
{
    partial class JsonReader : IStrategy
    {

        JObject GameJsonObj = JObject.Parse(File.ReadAllText(@"../LevelDataJson.json"));
        public Game CreateBasedOnFile()
        {

            return new Game(CreatePlayer(), CreateConnection(GameJsonObj), CreateRoom());
          
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
                List<Iitem> setItems = new List<Iitem>();
                if (room["items"] != null) { setItems = CreateRoomItems(JToken.FromObject(room["items"])); }
                roomList.Add(new Room
                {
                    id = room["id"].Value<int>(),
                    width = room["width"].Value<int>(),
                    height = room["height"].Value<int>(),
                    type = room["type"].Value<string>(),
                    items = setItems,
                    toggle = true
                });
            }
            return roomList;
        }

        public static List<Iitem> CreateRoomItems(JToken items)
        {
            List<Iitem> itemList = new List<Iitem>();
            foreach (var jsonItem in items)
            {
                
                ItemFactory itemFactory = new ItemFactory(jsonItem);
                itemList.Add(itemFactory.ProduceItems());
            }
            return itemList;
        }


        public static List<Connection> CreateConnection(JObject json)
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
        
        private static Door CreateDoors(JToken doorToken)
        {
            bool firstInitilizer = true;
            DoorDecorator? doorDecorator = null;
            foreach (var jsonItem in doorToken)
            {

                if (firstInitilizer == true)
                {
                    PlainDoor doorObj = new PlainDoor(doorToken);
                    string type = jsonItem["type"].Value<string>();
                    if (type.Equals("colored")) { doorDecorator = new ColoredDoorDecorator(doorObj, jsonItem["color"].Value<string>()); }
                    if (type.Equals("open on stones in room")) { doorDecorator = new OpenOnStonesInRoomDoorDecorator(doorObj, jsonItem["no_of_stones"].Value<int>()); }
                    if (type.Equals("open on odd")) { doorDecorator = new OpenOnOddDoorDecorator(doorObj); }
                    if (type.Equals("toggle")) { doorDecorator = new ToggleDoorDecorator(doorObj); }
                    if (type.Equals("closing gate")) { doorDecorator = new ClosingDoorDecorator(doorObj); }
                    firstInitilizer = false;
                    if (doorToken.Count() == 1) { return doorDecorator; }
                }
                else
                {
                    Door? xDecorator = null;
                    string type = jsonItem["type"].Value<string>();
                    if (type.Equals("colored")) { xDecorator = new ColoredDoorDecorator(doorDecorator, jsonItem["color"].Value<string>()); }
                    if (type.Equals("open on stones in room")) { doorDecorator = new OpenOnStonesInRoomDoorDecorator(doorDecorator, jsonItem["no_of_stones"].Value<int>()); }
                    if (type.Equals("open on odd")) { xDecorator = new OpenOnOddDoorDecorator(doorDecorator); }
                    if (type.Equals("toggle")) { xDecorator = new ToggleDoorDecorator(doorDecorator); }
                    if (type.Equals("closing gate")) { xDecorator = new ClosingDoorDecorator(doorDecorator); }
                    return xDecorator;
                }
            }
            return doorDecorator;
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
    }
}

