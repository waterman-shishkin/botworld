using System;
using botworld.bl;

namespace botworld.console
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var bot = new Bot("Console Bot", new ConsoleKeysSequenceSource());
			Console.WriteLine("Hello from bot \"{0}\"!", bot.Name);
			BotAction nextAction;
			do
			{
				nextAction = bot.ChooseNextAction();
				Console.WriteLine("Next command: \"{0}\"", nextAction);
			} while (nextAction != BotAction.None);
			Console.ReadKey();
		}
	}
}
