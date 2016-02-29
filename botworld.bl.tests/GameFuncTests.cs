using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class GameFuncTests
	{
		[Test]
		public void Tick_ForBotWhichDesireToTurnLeft_ResultsInBotRotation()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.TurnLeft);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.Direction, Is.EqualTo(Direction.West));
		}

		[Test]
		public void Tick_ForBotWhichDesireToTurnRight_ResultsInBotRotation()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.TurnRight);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.Direction, Is.EqualTo(Direction.East));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStep_ResultsInBotMovement()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.Location, Is.EqualTo(new Location(2, 3)));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepOutOfMapRange_DoNothing()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var location = new Location(0, 4);
			var bot = new Bot("Bot", 20, 10, 0, 5, location, Direction.West, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.Location, Is.EqualTo(location));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByGem_ResultsInBotMovement()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var targetLocation = new Location(2, 3);
			var gem = new Gem(100, targetLocation);
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(gem);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.Location, Is.EqualTo(targetLocation));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByWeakMine_ResultsInMineDestroyAndBotDamageAndBotMovement()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var targetLocation = new Location(2, 3);
			var mine = new Mine(10, targetLocation);
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(mine);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(mine.IsDead, Is.True);
			Assert.That(bot.HP, Is.EqualTo(15));
			Assert.That(bot.Location, Is.EqualTo(targetLocation));
		}

		// До бота, который умер в процессе, очередь не доходит
		// Step - перемещение на клетку, куда можно встать с боем (мина, бот)
		// Step - перемещение на клетку, куда можно встать с боем, но погибаем
		// Step - перемещение на клетку, куда нельзя встать
		// Step - перемещение на клетку, куда нельзя встать и погибаем
		// Step - перемещение на клетку, куда нельзя встать, но после боя становится можно
		// Step - перемещение на клетку, куда нельзя встать, но после боя становится можно, но погибаем
		// Act - атака на пустую клетку
		// Act - атака на клетку за границей
		// Act - атака на клетку, где не отвечают на атаку
		// Act - атака на клетку, где отвечают на атаку
		// Act - атака на клетку, где отвечают на атаку и погибаем
		// Collect - когда нечего собирать
		// Collect - собираем то, что можно собрать
		// Explore - исследование клетки
		// Explore - попытка исследовать клетку за границей
	}
}
