namespace Level_Data.StrategyPattern
{
    public class MainReader
    {
        public Game ReadGameData()
        {
            var context = new Context();

            /*Console.WriteLine("Client: Strategy is set to normal sorting.");
            context.SetStrategy(new JsonReader());
            //           context.GetGame();
*/

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            context.SetStrategy(new XmlReader());

            return context.GetGame();
        }
    }
}