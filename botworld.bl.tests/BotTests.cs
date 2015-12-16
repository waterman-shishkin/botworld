using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class BotTests
	{
		[Test]
		public void Name_Returns_NameSetByConstructor()
		{
			var bot = new Bot("Angry bot", null);

			Assert.That(bot.Name, Is.EqualTo("Angry bot"));
		}

		[Test]
		public void ChooseNextAction_ForKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = new[] { ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.Spacebar, ConsoleKey.Enter, ConsoleKey.NumPad0, ConsoleKey.LeftWindows };
			var bot = new Bot("Angry bot", new PredefinedKeysSequenceSource(keysSequence));

			var actions = new List<BotAction>();
			for (var i = 0; i < keysSequence.Length; i++)
				actions.Add(bot.ChooseNextAction());

			var expectedActions = new[] { BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None };

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
