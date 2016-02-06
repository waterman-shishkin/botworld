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
		public void ChooseInvasionResponseAction_Always_ReturnnsNone()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var action = botIntelligence.ChooseInvasionResponseAction(null, null);

			Assert.That(action, Is.EqualTo(InvasionResponseAction.None));
		}

		[Test]
		public void ChooseAttackResponseAction_Always_ReturnnsNone()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var action = botIntelligence.ChooseAttackResponseAction(null, null);

			Assert.That(action, Is.EqualTo(AttackResponseAction.None));
		}

		[Test]
		public void ChooseNextAction_ForNorthStartDirectionAndCertainKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			keysSequence.GetNextKey().Returns(
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.LeftArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.DownArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.Spacebar, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.Enter, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.NumPad0, false, false, false));
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var expectedActions = new[] { 
				BotAction.Explore, // Ctrl+Up
				BotAction.Step, // Up
				BotAction.TurnLeft, // Left
				BotAction.Step, 
				BotAction.TurnRight, // Right
				BotAction.Step, 
				BotAction.TurnRight, // Ctrl+Down
				BotAction.TurnRight, 
				BotAction.Explore, 
				BotAction.Act, // Spacebar
				BotAction.Collect, // Enter
				BotAction.None // NumPad0
			};
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(botIntelligence.ChooseNextAction(new BotInfo(100, 5, 3, new Location(1, 1), 10, Direction.North), null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseNextAction_ForEastStartDirectionAndCertainKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			keysSequence.GetNextKey().Returns(
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.LeftArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.DownArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.Spacebar, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.Enter, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.NumPad0, false, false, false));
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var expectedActions = new[] { 
				BotAction.TurnLeft, // Ctrl+Up
				BotAction.Explore,
				BotAction.TurnLeft, // Up
				BotAction.Step,
				BotAction.TurnRight, // Left
				BotAction.TurnRight,
				BotAction.Step, 
				BotAction.Step, // Right
				BotAction.TurnRight, // Ctrl+Down
				BotAction.Explore, 
				BotAction.Act, // Spacebar
				BotAction.Collect, // Enter
				BotAction.None // NumPad0
			};
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(botIntelligence.ChooseNextAction(new BotInfo(100, 5, 3, new Location(1, 1), 10, Direction.East), null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseNextAction_ForSouthStartDirectionAndCertainKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			keysSequence.GetNextKey().Returns(
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.LeftArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.DownArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.Spacebar, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.Enter, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.NumPad0, false, false, false));
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var expectedActions = new[] { 
				BotAction.TurnRight, // Ctrl+Up
				BotAction.TurnRight,
				BotAction.Explore,
				BotAction.TurnRight, // Up
				BotAction.TurnRight,
				BotAction.Step,
				BotAction.TurnRight, // Left
				BotAction.Step, 
				BotAction.TurnLeft, // Right
				BotAction.Step,
				BotAction.Explore, // Ctrl+Down
				BotAction.Act, // Spacebar
				BotAction.Collect, // Enter
				BotAction.None // NumPad0
			};
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(botIntelligence.ChooseNextAction(new BotInfo(100, 5, 3, new Location(1, 1), 10, Direction.South), null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseNextAction_ForWestStartDirectionAndCertainKeysSequence_ReturnsCorrespondingActions()
		{
			var keysSequence = Substitute.For<IKeysSequenceSource>();
			keysSequence.GetNextKey().Returns(
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.LeftArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.RightArrow, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.DownArrow, false, false, true),
				new ConsoleKeyInfo(default(char), ConsoleKey.Spacebar, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.Enter, false, false, false),
				new ConsoleKeyInfo(default(char), ConsoleKey.NumPad0, false, false, false));
			var botIntelligence = new HumanControlBotIntelligence(keysSequence);

			var expectedActions = new[] { 
				BotAction.TurnRight, // Ctrl+Up
				BotAction.Explore,
				BotAction.TurnRight, // Up
				BotAction.Step,
				BotAction.Step, // Left
				BotAction.TurnRight, // Right
				BotAction.TurnRight,
				BotAction.Step,
				BotAction.TurnLeft, // Ctrl+Down
				BotAction.Explore,
				BotAction.Act, // Spacebar
				BotAction.Collect, // Enter
				BotAction.None // NumPad0
			};
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(botIntelligence.ChooseNextAction(new BotInfo(100, 5, 3, new Location(1, 1), 10, Direction.West), null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
