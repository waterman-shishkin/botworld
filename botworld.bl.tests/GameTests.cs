using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class GameTests
	{
		[Test]
		public void GameOver_Returns_ValueFromScenario([Values(true, false)] bool gameOverValue)
		{
			var scenario = Substitute.For<IGameScenario>();
			scenario.IsGameOver(Arg.Any<IMap>()).Returns(gameOverValue);
			var game = new Game(null, scenario);

			Assert.That(game.GameOver, Is.EqualTo(gameOverValue));
		}

		[Test]
		public void Winners_Returns_ValueFromScenario()
		{
			var scenario = Substitute.For<IGameScenario>();
			var bot1 = Substitute.For<IBot>();
			var bot2 = Substitute.For<IBot>();
			var bots = new [] {bot1, bot2};
			scenario.GetWinners(Arg.Any<IMap>()).Returns(bots);
			var game = new Game(null, scenario);

			Assert.That(game.Winners, Is.EqualTo(bots));
		}

		[Test]
		public void Tick_DontAffectDeadBots()
		{
			var bot = Substitute.For<IBot>();
			bot.IsDead.Returns(true);
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(new[] { bot });
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			bot.Received(0).ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>());
		}

		[Test]
		public void Tick_ForBotWhichDesireToTurnLeft_AskMapToRotateBot()
		{
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			bot.Direction.Returns(Direction.North);
			bot.ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.TurnLeft);
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(new[] { bot });
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			map.Received(1).MoveBot(bot, location, Direction.West);
		}

		[Test]
		public void Tick_ForBotWhichDesireToTurnRight_AskMapToRotateBot()
		{
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			bot.Direction.Returns(Direction.North);
			bot.ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.TurnRight);
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(new[] { bot });
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			map.Received(1).MoveBot(bot, location, Direction.East);
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStep_AskMapToMoveBot()
		{
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			bot.Direction.Returns(Direction.North);
			bot.ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(new[] { bot });
			map.IsInRange(Arg.Any<Location>()).Returns(true);
			map.CanMoveBot(Arg.Any<IBot>(), Arg.Any<Location>()).Returns(true);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			map.Received(1).MoveBot(bot, new Location(2, 3), Direction.North);
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepOutOfMapRange_DoNothing()
		{
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(0, 4));
			bot.Direction.Returns(Direction.West);
			bot.ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(new[] { bot });
			map.IsInRange(Arg.Any<Location>()).Returns(false);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			map.Received(0).MoveBot(bot, Arg.Any<Location>(), Arg.Any<Direction>());
		}
	}
}
