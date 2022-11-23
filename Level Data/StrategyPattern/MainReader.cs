namespace Level_Data.StrategyPattern
{
    public class MainReader
    {

        public void ReadGameData()
        {
            // The client code picks a concrete strategy and passes it to the
            // context. The client should be aware of the differences between
            // strategies in order to make the right choice.
            var context = new Context();

            Console.WriteLine("Client: Strategy is set to normal sorting.");
            context.SetStrategy(new JsonReader());
            context.DoSomeBusinessLogic();

            Console.WriteLine();

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            context.SetStrategy(new XmlReader());
            context.DoSomeBusinessLogic();



        }


    }
}