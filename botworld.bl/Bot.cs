using System;

namespace botworld.bl
{
    public class Bot
    {
        public string Name { get; private set; }

        public Bot(string name)
        {
            Name = name;
        }

        public BotAction ChooseNextAction()
        {
            var keyPressed = Console.ReadKey().Key;
            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    return BotAction.Step;
                case ConsoleKey.LeftArrow:
                    return BotAction.TurnLeft;
                case ConsoleKey.RightArrow:
                    return BotAction.TurnRight;
                case ConsoleKey.Spacebar:
                    return BotAction.Act;
                case ConsoleKey.NumPad0:
                    return BotAction.Collect;
                case ConsoleKey.Enter:
                    return BotAction.Explore;
            }
            return BotAction.None;
        }
    }
}
