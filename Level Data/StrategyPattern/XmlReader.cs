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

            for (int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                int id = Int32.Parse(roomNode.ChildNodes[i].Attributes["id"].Value);
                int width = Int32.Parse(roomNode.ChildNodes[i].Attributes["width"].Value);
                int height = Int32.Parse(roomNode.ChildNodes[i].Attributes["height"].Value);
                List<Iitem> items = CreateRoomItems(roomNode.ChildNodes[i]);
            }
            List<Room> roomList = new List<Room>();
            return roomList;
        }

        private static List<Iitem> CreateRoomItems(XmlNode room)
        {
            List<XmlNode> lijstje = new List<XmlNode>();
            XmlDocument roomsDoc = new XmlDocument();
            List <Iitem> items = new List<Iitem>();
            
                if (room.InnerXml != "")
                {
                    roomsDoc.LoadXml(room.InnerXml);
                    XmlNode itemNode = roomsDoc.DocumentElement.SelectSingleNode("/items");

                    for (int i = 0; i < itemNode.ChildNodes.Count; i++)
                    {   
                    lijstje.Add(itemNode.ChildNodes[i]);
                    ItemFactory itemFactory = new ItemFactory(itemNode.ChildNodes[i]);
                    }
                }
            return items;
        }



    }
}
      

    



