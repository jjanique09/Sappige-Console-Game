using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Level_Data
{
    class JsonReader : IStrategy
    {
        const string JsonPath = @"../LevelDataJson.json";
        JObject GameJsonObj = JObject.Parse(File.ReadAllText(JsonPath));


        public object DoAlgorithm(object data)
        {





            var list = data as List<string>;
            list.Sort();

            return list;
        }

    /*   public Player CreatePlayer()
         {
            string a = 

            var player = json["player"];
            var startRoomId = player["startRoomId"].Value<int>();
            var x = player["startX"].Value<int>();
            var y = player["startY"].Value<int>();
            var lives = player["lives"].Value<int>();
        
            return new Player(startRoomId, startPosition, player["lives"].Value<int>());
        }*/


        public class Player
        {

            public int startRoomId { get; set; }
            public int startX { get; set; }
            public int startY { get; set; }
            public int lives { get; set; }

        }
	}
}