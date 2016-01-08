using System;

namespace botworld.bl
{
	public class HumanControlBotIntelligence : IBotIntelligence
	{
		private readonly IKeysSequenceSource keysSource;

		public HumanControlBotIntelligence(IKeysSequenceSource keysSource)
		{
			this.keysSource = keysSource;
		}

		public BotAction ChooseNextAction(BotInfo botInfo)
		{
			var keyPressed = keysSource.GetNextKey();
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