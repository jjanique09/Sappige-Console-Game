using Newtonsoft.Json;

namespace Level_Data
{
    class JsonReader : IStrategy
    {
        public object DoAlgorithm(object data)
        {

            LoadJson();
            var list = data as List<string>;
            list.Sort();

            return list;
        }

        public void LoadJson()
        {
            StreamReader r = new StreamReader("C:\\Users\\Administrator\\Documents\\GitHub\\Sappige-Console-Game\\Level Data\\LevelDataFiles\\LevelDataJson.json");
            string json = r.ReadToEnd();

            Player objResponse1 = JsonConvert.DeserializeObject<Player>(json);

            Console.WriteLine("Sappige test " + objResponse1.startY);



        }

        public class Player
        {
            public int startRoomId;
            public int startX;
            public int startY;
            public int lives;
          
        }
	}
}