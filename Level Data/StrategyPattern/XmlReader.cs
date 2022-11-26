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
           
           
            List<Room> roomList = new List<Room>();
            for(int i = 0; i < roomNode.ChildNodes.Count; i++)
            {
                roomList.Add(new Room
                {
                    id = Int32.Parse(roomNode.ChildNodes[0].Attributes["id"].Value),
                    width = 0,
                    height = 0,
                    type = null,
                    items = null,
                    toggle = false,

                }) ;
            }



           /* foreach (var room in GameJsonObj["rooms"])
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
            }*/
           




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


    }
}


