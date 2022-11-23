using Level_Data.StrategyPattern;

namespace Game_Logica
{
    class Program
    {
        static void Main(string[] args)
        {
            MainReader mainReader = new MainReader();
            mainReader.ReadGameData();
        }

    }

}