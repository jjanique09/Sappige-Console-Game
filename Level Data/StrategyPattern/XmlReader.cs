using Newtonsoft.Json.Linq;
using System.Xml;


namespace Level_Data.StrategyPattern
{
    class XmlReader : IStrategy
    {
        JObject GameJsonObj;

        public XmlReader()
        {
            //https://stackoverflow.com/questions/642293/how-do-i-read-and-parse-an-xml-file-in-c

            XmlDocument doc = new XmlDocument();
            doc.Load(@"../LevelDataXml.xml");

            XmlNode node = doc.DocumentElement.SelectSingleNode("/temple");
            //var RoomId = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "player lives").First().InnerText;


            // https://stackoverflow.com/questions/13605396/c-sharp-get-xml-tag-value
            
            
            Console.WriteLine(node.FirstChild);
        }

        public Game CreateBasedOnFile()
        {
            return new Game(null, null, null);
        }


        public Player CreatePlayer(XmlDocument doc)
        {

            XmlNode node = doc.DocumentElement.SelectSingleNode("/temple/player/start");
            XmlNode templeNode = doc.DocumentElement.SelectSingleNode("/temple/player");

            return new Player
            {
                startY = Int32.Parse(node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "y").First().InnerText),
                startX = Int32.Parse(node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "x").First().InnerText),
                startRoomId = Int32.Parse(node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "roomId").First().InnerText),
                lives = Int32.Parse(doc.GetElementsByTagName("player")[0].Attributes["lives"].Value),

        };
        }
    }
}


