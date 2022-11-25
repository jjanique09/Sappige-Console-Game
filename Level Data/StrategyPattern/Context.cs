namespace Level_Data.StrategyPattern
{

    class Context
    {
        private IStrategy _strategy;

        public Context()
        { }

        public Context(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public Game GetGame()
        {
            return _strategy.CreateBasedOnFile();
        }
    }
}
