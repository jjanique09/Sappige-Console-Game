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
    }



    public List<Room> CreateRoom(XmlDocument doc)
    {
        XmlNodeList roomNode = doc.DocumentElement.SelectNodes("/temple/rooms");
        var test2 = roomNode[2].Attributes["items"].Value;
        var test3 = roomNode[3].Attributes["items"].Value;
        Console.ReadKey();

        List<Room> roomList = new List<Room>();
        for (int i = 0; i < roomNode.Count; i++)
        {
            List<Iitem> setItems = new List<Iitem>();

            roomList.Add(new Room
            {
                id = Int32.Parse(roomNode[i].Attributes["id"].Value),
                width = Int32.Parse(roomNode[i].Attributes["width"].Value),
                height = Int32.Parse(roomNode[i].Attributes["height"].Value),
                type = null,
                items = setItems,
                toggle = true,

            });



            return roomList;
        }
    }

        public static List<Iitem> CreateRoomItems(XmlNode ItemNode)
        {
            List<Iitem> itemList = new List<Iitem>();
            foreach (var node in ItemNode)
            {
      /*          ItemFactory itemFactory = new ItemFactory(jsonItem);
                itemList.Add(itemFactory.ProduceItems());*/
            }
            return itemList;
        }


    
}


