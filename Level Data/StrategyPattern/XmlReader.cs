using Newtonsoft.Json.Linq;
using System.Xml;


namespace Level_Data.StrategyPattern
{
    class XmlReader : IStrategy
    {
        JObject GameJsonObj;

        public XmlReader()
        {
            // https://stackoverflow.com/questions/642293/how-do-i-read-and-parse-an-xml-file-in-c
            // https://stackoverflow.com/questions/13605396/c-sharp-get-xml-tag-value

            XmlDocument doc = new XmlDocument();
            doc.Load(@"../LevelDataXml.xml");
            CreateRoom(doc);
            CreateConnection(doc);
        }


        public Game CreateBasedOnFile()
        {
            return new Game(null, null, null);
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
                roomList.Add(new Room
                {
                    id = Int32.Parse(roomNode.ChildNodes[i].Attributes["id"].Value),
                    width = Int32.Parse(roomNode.ChildNodes[i].Attributes["width"].Value),
                    height = Int32.Parse(roomNode.ChildNodes[i].Attributes["height"].Value),
                    items = CreateRoomItems(roomNode.ChildNodes[i])
                });
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
                connectionList.Add(setConnection);
            }
           
            return connectionList;
        }
    }

}

      

    



