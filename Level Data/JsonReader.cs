using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Level_Data
{
    class JsonReader : IStrategy
    {
        JObject GameJsonObj = JObject.Parse(File.ReadAllText(@"../LevelDataJson.json"));

        public object DoAlgorithm(object data)
        {
            Console.WriteLine("Test " + CreatePlayer().startX);
            var list = data as List<string>;
            list.Sort();
            return list;
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
                List<Item> setItems = new List<Item>();
                if (room["items"] != null) { setItems = CreateRoomItems(JToken.FromObject(room["items"])); }
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

        private List<Item> CreateRoomItems(JToken items)
        {
            List<Item> itemList = new List<Item>();
            foreach (var jsonItem in items)
            {
                itemList.Add(new Item
                 {
                    type = jsonItem["type"].Value<string>(),
                    color = (jsonItem["color"] ?? "").Value<string>(),
                    damage = (jsonItem["int"] ?? "").Value<int>(),
                    x = jsonItem["x"].Value<int>(),
                    y = jsonItem["y"].Value<int>(),
            });
            }
            return itemList;
        }


        public class Item 
        {
           public string type { get; set; }
           public string color { get; set; }
           public int x { get; set; }
           public int y { get; set; }
           public int damage { get; set; }
        }



        public class Room
        {
            public int id { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string type { get; set; }
            public List<Item> items { get; set; }
        }





public class Player
        {
            public int startRoomId { get; set; }
            public int startX { get; set; }
            public int startY { get; set; }
            public int lives { get; set; }
        }
	}
}