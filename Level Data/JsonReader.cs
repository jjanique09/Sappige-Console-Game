
using Newtonsoft.Json.Linq;
using static Level_Data.JsonReader.ItemFactory;

namespace Level_Data
{
    class JsonReader : IStrategy
    {

        JObject GameJsonObj = JObject.Parse(File.ReadAllText("C:\\Users\\joram\\Documents\\LevelDataJson.json"));
        public object DoAlgorithm(object data)
        {
            List<Connection> connections = ItemFactory.CreateConnection(GameJsonObj);
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
            public ColoredDoorDecorator(Door door, string color) : base(door)
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
                if (door.Open(game) == true && game.player.items.Where(x => x.type.Equals("sankara stone")).Count() % 2 != 0)
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
            public OpenOnStonesInRoomDoorDecorator(Door door, int setNumberOfStones) : base(door)
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
                List<Iitem> setItems = new List<Iitem>();
                if (room["items"] != null) { setItems = ItemFactory.CreateRoomItems(JToken.FromObject(room["items"])); }
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

        public interface Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public void Use(Game game);
        }

        public class SankaraStoneItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public SankaraStoneItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.items.Add(new SankaraStoneItem(type, color, x, y, damage));
            }
        }


        public class KeyItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public KeyItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.items.Add(new SankaraStoneItem(type, color, x, y, damage));
            }
        }


        public class BoobyTrapItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public BoobyTrapItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.lives -= damage.Value;
            }
        }


        public class DisapearingBoobyTrapItem : Iitem
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public DisapearingBoobyTrapItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.rooms.Where(x => x.id == game.player.startRoomId).First().items.RemoveAll(e => e.type.Equals("disappearing boobytrap") && e.x == x && e.y == y);
                game.player.lives -= damage.Value;
            }
        }

        public class PressurePlateItem : Iitem // What the hell does this do?
        {
            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public PressurePlateItem(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public void Use(Game game)
            {
                game.player.items.Add(new PressurePlateItem(type, color, x, y, damage));
            }
        };

        public class ItemFactory
        {

            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }

            public ItemFactory(string setType, string? setColor, int setX, int setY, int? setDamage)
            {
                type = setType;
                color = setColor;
                x = setX;
                y = setY;
                damage = setDamage;
            }

            public Iitem CreateItem()
            {
                if ("sankara stone".Equals(type))
                {
                    return new SankaraStoneItem(type, color, x, y, damage);
                }

                else if ("sankara stone".Equals(type))
                {
                    return new BoobyTrapItem(type, color, x, y, damage);
                }

                else if ("sankara stone".Equals(type))
                {
                    return new DisapearingBoobyTrapItem(type, color, x, y, damage);
                }

                else if ("sankara stone".Equals(type))
                {
                    return new PressurePlateItem(type, color, x, y, damage);
                }

                else if ("sankara stone".Equals(type))
                {
                    return new SankaraStoneItem(type, color, x, y, damage);
                }

                else if ("key".Equals(type))
                {
                    return new KeyItem(type, color, x, y, damage);
                }

                return null;

            }


            public static List<Iitem> CreateRoomItems(JToken items)  /// This will become an Item Factory ---> https://www.c-sharpcorner.com/article/factory-design-pattern-in-c-sharp/
            {
                List<Iitem> itemList = new List<Iitem>();
                foreach (var jsonItem in items)
                {
                    ItemFactory itemFactory = new ItemFactory(jsonItem["type"].Value<string>(), jsonItem["color"].Value<string>(), jsonItem["x"].Value<int>(), jsonItem["y"].Value<int>(), jsonItem["damage"].Value<int>());
                    itemList.Add(itemFactory.CreateItem());
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
                Console.WriteLine("");
                foreach (var jsonItem in doorToken)
                {

                    if (firstInitilizer == true)
                    {
                        PlainDoor doorObj = new PlainDoor(doorToken);
                        //Door? doorObj = new PlainDoor(doorToken);
                        string type = jsonItem["type"].Value<string>();
                        if (type.Equals("colored")) { doorDecorator = new ColoredDoorDecorator(doorObj, jsonItem["color"].Value<string>()); }
                        if (type.Equals("open on stones in room")) { doorDecorator = new OpenOnStonesInRoomDoorDecorator(doorObj, jsonItem["no_of_stones"].Value<int>()); }
                        if (type.Equals("open on odd")) { doorDecorator = new OpenOnOddDoorDecorator(doorObj); }
                        if (type.Equals("toggle")) { doorDecorator = new ToggleDoorDecorator(doorObj); }
                        if (type.Equals("closing gate")) { doorDecorator = new ClosingDoorDecorator(doorObj); }


                        //Uncomment check if continuity is still correct ---> // Console.WriteLine("Dit moet een 1 zijn verdikkie " + doorToken.Count());
                        //Console.WriteLine(doorToken.Count());
                        firstInitilizer = false;
                        if (doorToken.Count() == 1) { return doorDecorator; }
                    }
                    else
                    {
                        //Uncomment check if continuity is still correct ---> // Console.WriteLine("Dit mag geen 1 zijn verdikkie " + doorToken.Count());
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


            /*        public class Item 
                    {
                       public string type { get; set; }
                       public string? color { get; set; }
                       public int x { get; set; }
                       public int y { get; set; }
                       public int? damage { get; set; }
                    }
            */

            public class Room
            {
                public int id { get; set; }
                public int width { get; set; }
                public int height { get; set; }
                public string type { get; set; }
                public List<Iitem> items { get; set; }
            }


            public class Player
            {
                public int startRoomId { get; set; }
                public int startX { get; set; }
                public int startY { get; set; }
                public int lives { get; set; }
                public List<Iitem> items { get; set; }
            }
        }
    }
}

