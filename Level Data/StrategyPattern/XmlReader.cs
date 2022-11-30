using Newtonsoft.Json.Linq;
using System.Xml;


namespace Level_Data.StrategyPattern
{
    class XmlReader : IStrategy
    {
        JObject GameJsonObj;

        public XmlReader()
        {

        }

        public Game CreateBasedOnFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"../LevelDataXml.xml");
            Game game = new Game(CreatePlayer(doc), CreateConnection(doc), CreateRoom(doc));
            return game;
        }
        
        public Player CreatePlayer(XmlDocument doc)
        {

            XmlNode playerNode = doc.DocumentElement.SelectSingleNode("/temple/player/start");

            return new Player
            {
                startY = Int32.Parse(playerNode.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "y").First().InnerText),
                startX = Int32.Parse(playerNode.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "x").First().InnerText),
                startRoomId = Int32.Parse(playerNode.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "roomId").First().InnerText),
                lives = Int32.Parse(doc.GetElementsByTagName("player")[0].Attributes["lives"].Value),
            };
        }

        public List<Room> CreateRoom(XmlDocument doc)
        {
             XmlNode roomNode = doc.DocumentElement.SelectSingleNode("/temple/rooms");
             List<Room> roomList = new List<Room>();


            for (int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                List<Iitem> setItems = new List<Iitem>();
                if (roomNode.ChildNodes[i].InnerXml != "") { setItems = CreateRoomItems(roomNode.ChildNodes[i]); }
                roomList.Add(new Room
                {
                    id = Int32.Parse(roomNode.ChildNodes[i].Attributes["id"].Value),
                    width = Int32.Parse(roomNode.ChildNodes[i].Attributes["width"].Value),
                    height = Int32.Parse(roomNode.ChildNodes[i].Attributes["height"].Value),
                    items = setItems
                }) ;
            }
            return roomList;
        }

        private static List<Iitem> CreateRoomItems(XmlNode room)
        {
            XmlDocument roomsDoc = new XmlDocument();
            List <Iitem> items = new List<Iitem>();
            
                if (room.InnerXml != "")
                {
                    roomsDoc.LoadXml(room.InnerXml);
                    XmlNode itemNode = roomsDoc.DocumentElement.SelectSingleNode("/items");

                    for (int i = 0; i < itemNode.ChildNodes.Count; i++)
                    {   
                    ItemFactory itemFactory = new ItemFactory(itemNode.ChildNodes[i]);
                    items.Add(itemFactory.ProduceItems());
                    }
                }
            return items;
        }

        public static List<Connection> CreateConnection(XmlDocument doc)
        {
            XmlNode connectionNode = doc.DocumentElement.SelectSingleNode("/temple/connections");
            List<Connection> connectionList = new List<Connection>();
            
            for (int i = 0; i < connectionNode.ChildNodes.Count; i++)
            {
                Connection setConnection = new Connection();
                if (connectionNode.ChildNodes[i].Attributes["NORTH"] != null ) { setConnection.north = Int32.Parse(connectionNode.ChildNodes[i].Attributes["NORTH"].Value); Console.WriteLine(setConnection.north); }
                if (connectionNode.ChildNodes[i].Attributes["SOUTH"] != null ) { setConnection.south = Int32.Parse(connectionNode.ChildNodes[i].Attributes["SOUTH"].Value); Console.WriteLine(setConnection.south); }
                if (connectionNode.ChildNodes[i].Attributes["WEST"] != null ) { setConnection.west = Int32.Parse(connectionNode.ChildNodes[i].Attributes["WEST"].Value); Console.WriteLine(setConnection.west); }
                if (connectionNode.ChildNodes[i].Attributes["EAST"] != null ) { setConnection.east = Int32.Parse(connectionNode.ChildNodes[i].Attributes["EAST"].Value); Console.WriteLine(setConnection.east); }
                if (connectionNode.ChildNodes[i].InnerXml != "") { setConnection.door = CreateRoomDoors(connectionNode.ChildNodes[i]); }
                connectionList.Add(setConnection);
            }
            return connectionList;
        }


        private static Door CreateRoomDoors(XmlNode room)
        {
            XmlDocument roomsDoc = new XmlDocument();
            bool firstInitilizer = true;
            DoorDecorator? doorDecorator = null;

            if (room.InnerXml != "")
            {
                roomsDoc.LoadXml(room.InnerXml);
                XmlNode itemNode = roomsDoc.DocumentElement.SelectSingleNode("/doors");

                for (int i = 0; i < itemNode.ChildNodes.Count; i++)
                {

                    if (firstInitilizer == true)
                    {
                        PlainDoor doorObj = new PlainDoor();
                        string type = itemNode.ChildNodes[i].LocalName;
                        if (type.Equals("colouredDoor")) { doorDecorator = new ColoredDoorDecorator(doorObj, itemNode.ChildNodes[i].Attributes["color"].Value); }
                        if (type.Equals("openOnStoneInRoomDoor")) { doorDecorator = new OpenOnStonesInRoomDoorDecorator(doorObj, Int32.Parse(itemNode.ChildNodes[i].Attributes["no_of_stones"].Value)); }
                        if (type.Equals("openOnOddLivesDoor")) { doorDecorator = new OpenOnOddDoorDecorator(doorObj); }
                        if (type.Equals("toggleDoor")) { doorDecorator = new ToggleDoorDecorator(doorObj); }
                        if (type.Equals("closingGate")) { doorDecorator = new ClosingDoorDecorator(doorObj); }
                        firstInitilizer = false;
                        if (itemNode.ChildNodes.Count == 1) { return doorDecorator; }
                    }
                    else
                    {
                        Door? xDecorator = null;
                        string type = itemNode.ChildNodes[i].LocalName;
                        if (type.Equals("colouredDoor")) { xDecorator = new ColoredDoorDecorator(doorDecorator, itemNode.ChildNodes[i].Attributes["color"].Value); }
                        if (type.Equals("openOnStoneInRoomDoor")) { doorDecorator = new OpenOnStonesInRoomDoorDecorator(doorDecorator, Int32.Parse(itemNode.ChildNodes[i].Attributes["no_of_stones"].Value)); }
                        if (type.Equals("openOnOddLivesDoor")) { xDecorator = new OpenOnOddDoorDecorator(doorDecorator); }
                        if (type.Equals("toggleDoor")) { xDecorator = new ToggleDoorDecorator(doorDecorator); }
                        if (type.Equals("closingGate")) { xDecorator = new ClosingDoorDecorator(doorDecorator); }
                        return xDecorator;
                    }
                }
            }
            return doorDecorator;
        }
    }

}

      

    



