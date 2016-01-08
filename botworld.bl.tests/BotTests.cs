using System.Collections.Generic;
using NSubstitute;
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
		public void ChooseNextAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>()).Returns(BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None);
			var bot = new Bot("Angry bot", botIntelligence);

			var expectedActions = new[] { BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None };
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseNextAction());

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
