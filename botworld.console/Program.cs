using System;
using botworld.bl;

namespace botworld.console
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var botIntelligence = new HumanControlBotIntelligence(new ConsoleKeysSequenceSource());
			var bot = new Bot("Console Bot", 100, 5, 3, new Location(1, 1), Direction.North, botIntelligence);
			Console.WriteLine("Hello from bot \"{0}\"!", bot.Name);
			Console.WriteLine("My direction is \"{0}\"", bot.Direction);
			BotAction nextAction;
			do
			{
				nextAction = bot.ChooseNextAction();
				Console.WriteLine("Next command: \"{0}\"", nextAction);
				var newDirection = (int)bot.Direction;
				if (nextAction == BotAction.TurnLeft || nextAction == BotAction.TurnRight)
				{
					switch (nextAction)
					{
						case BotAction.TurnLeft:
							newDirection--;
							if (newDirection < 0)
								newDirection += 4;
							break;
						case BotAction.TurnRight:
							newDirection++;
							if (newDirection > 3)
								newDirection -= 4;
							break;
					}
				}
				bot.UpdateDirection((Direction)newDirection);
				Console.WriteLine("New direction: \"{0}\"", bot.Direction);
			} while (nextAction != BotAction.None);
			Console.ReadKey();
		}
	}
}
