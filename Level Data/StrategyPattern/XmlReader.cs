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

            List<XmlNode> xmls = new List<XmlNode>();
            
            XmlNode roomNode = doc.DocumentElement.SelectSingleNode("/temple/rooms");
            List<int> ids = new List<int>();
            for (int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                int id = Int32.Parse(roomNode.ChildNodes[i].Attributes["id"].Value);
                ids.Add(id);
              
            }

            for (int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                xmls.Add(roomNode.ChildNodes[i]);
                Console.WriteLine(ids);
            }



         /*   "<items><disappearing_boobytrap x=\"2\" y=\"1\" damage=\"1\" /><sankara_stone x=\"2\" y=\"2\" /></items>"*/

            XmlDocument rooms = new XmlDocument();
            foreach (var node in xmls)
            {
                if (node.InnerXml != "") {
                    rooms.LoadXml (node.InnerXml);
                    XmlNode itemNode = rooms.DocumentElement.SelectSingleNode("/items");

                    foreach (var item in itemNode)
                    {
                        int x = Int32.Parse(itemNode.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "x").First().InnerText);
                       
                        Console.WriteLine(x);
                        }
                }
                }



            List<XmlNode> Axls = xmls;
            List<Room> roomList = new List<Room>();

            return roomList;
        }

    }
}
      

    



