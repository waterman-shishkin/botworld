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

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByAgressiveBot_ResultsInBotDamageAndMovement()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var guestBot = new Bot("Guest", 20, 10, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var targetLocation = new Location(2, 3);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(InvasionResponseAction.Attack);
			var hostBot = new Bot("Host", 20, 10, 0, 5, targetLocation, Direction.North, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(guestBot.HP, Is.EqualTo(15));
			Assert.That(guestBot.Location, Is.EqualTo(targetLocation));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByStrongMine_ResultsInMineDestroyAndBotDeath()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var location = new Location(2, 4);
			var bot = new Bot("Bot", 20, 10, 0, 5, location, Direction.North, botIntelligence);
			var mine = new Mine(100, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(mine);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(mine.IsDead, Is.True);
			Assert.That(bot.IsDead, Is.True);
			Assert.That(bot.Location, Is.EqualTo(location));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByStrongAgressiveBot_ResultsInBotDeath()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var location = new Location(2, 4);
			var guestBot = new Bot("Guest", 3, 10, 0, 5, location, Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(InvasionResponseAction.Attack);
			var hostBot = new Bot("Host", 20, 10, 0, 5, new Location(2, 3), Direction.North, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(guestBot.IsDead, Is.True);
			Assert.That(guestBot.Location, Is.EqualTo(location));
		}

		[Test]
		public void Tick_ForStrongBotWhichDesireToMakeStepToCellOccupiedByWeakAgressiveBot_ResultsInGuestBotDamageAndMovementAndHostBotDeath()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			guestBotIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var guestBot = new Bot("Guest", 20, 100, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			hostBotIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(InvasionResponseAction.Attack);
			var targetLocation = new Location(2, 3);
			var hostBot = new Bot("Host", 20, 10, 0, 5, targetLocation, Direction.North, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(hostBot.IsDead, Is.True);
			Assert.That(hostBot.Location, Is.EqualTo(targetLocation));
			Assert.That(guestBot.HP, Is.EqualTo(15));
			Assert.That(guestBot.Location, Is.EqualTo(targetLocation));
		}

		[Test]
		public void Tick_ForBotWhichDesireToMakeStepToCellOccupiedByWall_ResultsInBotDamage()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var location = new Location(2, 4);
			var bot = new Bot("Bot", 20, 10, 0, 5, location, Direction.North, botIntelligence);
			var wall = new Wall(100, 20, 10, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(wall);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(wall.HP, Is.EqualTo(100));
			Assert.That(bot.HP, Is.EqualTo(5));
			Assert.That(bot.Location, Is.EqualTo(location));
		}

		[Test]
		public void Tick_ForWeakBotWhichDesireToMakeStepToCellOccupiedByWall_ResultsInBotDeath()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			var location = new Location(2, 4);
			var bot = new Bot("Bot", 10, 10, 0, 5, location, Direction.North, botIntelligence);
			var wall = new Wall(100, 20, 10, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(wall);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(wall.HP, Is.EqualTo(100));
			Assert.That(bot.IsDead, Is.True);
			Assert.That(bot.Location, Is.EqualTo(location));
		}

		[Test]
		public void Tick_ForStrongAgressiveBotWhichDesireToMakeStepToCellOccupiedByWall_ResultsInBotDamageAndMovementAndWallDestroy()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			botIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var bot = new Bot("Bot", 20, 100, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var targetLocation = new Location(2, 3);
			var wall = new Wall(10, 20, 10, targetLocation);
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(wall);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(wall.IsDead, Is.True);
			Assert.That(bot.HP, Is.EqualTo(5));
			Assert.That(bot.Location, Is.EqualTo(targetLocation));
		}

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
