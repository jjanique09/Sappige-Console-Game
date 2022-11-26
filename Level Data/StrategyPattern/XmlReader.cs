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
        /**//*if (roomNode.ChildNodes[8].Attributes["items"].Value != null) { setItems = CreateRoomItems(roomNode.ChildNodes[i].Attributes["items"]);*/




        public List<Room> CreateRoom(XmlDocument doc)
        {

            List<XmlNode> rooms = new List<XmlNode>();
            
            XmlNode roomNode = doc.DocumentElement.SelectSingleNode("/temple/rooms");
           
            for (int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                int id = Int32.Parse(roomNode.ChildNodes[i].Attributes["id"].Value);
                int width = Int32.Parse(roomNode.ChildNodes[i].Attributes["width"].Value);
                int height = Int32.Parse(roomNode.ChildNodes[i].Attributes["height"].Value);
                rooms.Add(roomNode.ChildNodes[i]);

            }

            List<XmlNode> lijstje = new List<XmlNode>();
            XmlDocument roomsDoc = new XmlDocument();
            foreach (var room in rooms) // each room
            {
                if (room.InnerXml != "") {
                    roomsDoc.LoadXml (room.InnerXml);
                    XmlNode itemNode = roomsDoc.DocumentElement.SelectSingleNode("/items");

                    for(int i = 0; i < itemNode.ChildNodes.Count; i++)
                    {
                        lijstje.Add(itemNode.ChildNodes[i]);
                        int x = Int32.Parse(itemNode.ChildNodes[i].Attributes["x"].Value);
                        int y = Int32.Parse(itemNode.ChildNodes[i].Attributes["y"].Value);
                        string type = itemNode.ChildNodes[i].LocalName;

                        try
                        {
                            int damage = Int32.Parse(itemNode.ChildNodes[i].Attributes["damage"].Value);
                        }
                        catch
                        {
                            int damage = 0;
                        }

                        try
                        {
                            string color = (itemNode.ChildNodes[i].Attributes["color"].ToString());
                        }
                        catch
                        {
                            string color = "";
                        }

                    }
                 }
                }


          
            List<Room> roomList = new List<Room>();

            return roomList;
        }

    }
}
      

    



