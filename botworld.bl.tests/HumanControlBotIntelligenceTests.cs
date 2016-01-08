using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class HumanControlBotIntelligenceTests
	{
		[Test]
		public void ChooseNextAction_ForKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			keysSequence.GetNextKey().Returns(ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.Spacebar, ConsoleKey.Enter, ConsoleKey.NumPad0, ConsoleKey.LeftWindows);
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var expectedActions = new[] { BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None };
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(botIntelligence.ChooseNextAction(null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
