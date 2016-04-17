using System.Collections.Generic;
using System.Linq;
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

		[Test]
		public void Tick_ForAgressiveBotWhichDesireToMakeStepToCellOccupiedByTwoAgressiveBots_ResultsInGuestBotDamageAndMovementAndHostBotsDamage()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			guestBotIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var guestBot = new Bot("Guest", 100, 10, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			hostBotIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(InvasionResponseAction.Attack);
			var targetLocation = new Location(2, 3);
			var hostBot1 = new Bot("Host-1", 90, 10, 0, 5, targetLocation, Direction.West, hostBotIntelligence);
			var hostBot2 = new Bot("Host-2", 80, 10, 0, 5, targetLocation, Direction.East, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot1);
			map.Add(hostBot2);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(hostBot1.HP, Is.EqualTo(85));
			Assert.That(hostBot1.Location, Is.EqualTo(new Location(1, 3)));
			Assert.That(hostBot2.HP, Is.EqualTo(75));
			Assert.That(hostBot2.Location, Is.EqualTo(new Location(3, 3)));
			Assert.That(guestBot.HP, Is.EqualTo(90));
			Assert.That(guestBot.Location, Is.EqualTo(targetLocation));
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnEmptyCell_DoNotThrow()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			TestDelegate testDelegate = () => game.Tick();

			Assert.DoesNotThrow(testDelegate);
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOutsideOfMap_DoNotThrow()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(0, 0), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			TestDelegate testDelegate = () => game.Tick();

			Assert.DoesNotThrow(testDelegate);
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnGem_ResultsInGemDestroy()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var gem = new Gem(100, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(gem); 
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.HP, Is.EqualTo(20));
			Assert.That(bot.WP, Is.EqualTo(0));
			Assert.That(gem.IsDead, Is.True);
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnMine_ResultsInMineDestroy()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var mine = new Mine(100, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(mine); 
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.HP, Is.EqualTo(20));
			Assert.That(mine.IsDead, Is.True);
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnWeakWall_ResultsInWallDestroy()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 20, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var wall = new Wall(10, 100, 5, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(wall); 
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.HP, Is.EqualTo(20));
			Assert.That(wall.IsDead, Is.True);
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnStrongWall_ResultsInWallDamage()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var wall = new Wall(100, 100, 5, new Location(2, 3));
			var map = new Map(10, 20);
			map.Add(bot);
			map.Add(wall); 
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot.HP, Is.EqualTo(20));
			Assert.That(wall.HP, Is.EqualTo(95));
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnAgressiveBot_ResultsInGuestBotDamageAndHostBotDamage()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var guestBot = new Bot("Guest", 100, 10, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			hostBotIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var hostBot = new Bot("Host", 90, 10, 0, 5, new Location(2, 3), Direction.West, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(guestBot.HP, Is.EqualTo(95));
			Assert.That(hostBot.HP, Is.EqualTo(85));
			Assert.That(hostBot.Location, Is.EqualTo(new Location(1, 3)));
		}

		[Test]
		public void Tick_ForBotWhichDesireToActOnCellOccupiedByTwoAgressiveWeakAndStrongBots_ResultsInGuestBotDamageAndAndHostStrongBotDamageAndHostWeakBotDeath()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var guestBot = new Bot("Guest", 100, 20, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			hostBotIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var targetLocation = new Location(2, 3);
			var hostBot1 = new Bot("Host-1", 90, 10, 0, 5, targetLocation, Direction.West, hostBotIntelligence);
			var hostBot2 = new Bot("Host-2", 10, 10, 0, 5, targetLocation, Direction.East, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot1);
			map.Add(hostBot2);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(guestBot.HP, Is.EqualTo(95));
			Assert.That(hostBot1.HP, Is.EqualTo(75));
			Assert.That(hostBot1.Location, Is.EqualTo(new Location(1, 3)));
			Assert.That(hostBot2.IsDead, Is.True);
			Assert.That(hostBot2.Location, Is.EqualTo(targetLocation));
		}

		[Test]
		public void Tick_ForWeakBotWhichDesireToActOnStrongAgressiveBot_ResultsInGuestBotDeathAndHostBotDamage()
		{
			var guestBotIntelligence = Substitute.For<IBotIntelligence>();
			guestBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Act);
			var guestBot = new Bot("Guest", 20, 10, 0, 5, new Location(2, 4), Direction.North, guestBotIntelligence);
			var hostBotIntelligence = Substitute.For<IBotIntelligence>();
			hostBotIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Step);
			hostBotIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).ReturnsForAnyArgs(AttackResponseAction.Attack);
			var hostBot = new Bot("Host", 100, 50, 0, 5, new Location(2, 3), Direction.West, hostBotIntelligence);
			var map = new Map(10, 20);
			map.Add(guestBot);
			map.Add(hostBot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(guestBot.IsDead, Is.True);
			Assert.That(hostBot.HP, Is.EqualTo(95));
			Assert.That(hostBot.Location, Is.EqualTo(new Location(1, 3)));
		}

		[Test]
		public void Tick_ForBotWhichDesireToCollectAtEmptyCell_DoNotThrow()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Collect);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(2, 4), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			TestDelegate testDelegate = () => game.Tick();

			Assert.DoesNotThrow(testDelegate);
		}

		[Test]
		public void Tick_ForBotWhichDesireToCollectAtCellContainingNothingToCollect_DoNotThrow()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Collect);
			var location = new Location(2, 4);
			var bot1 = new Bot("Bot-1", 20, 10, 0, 5, location, Direction.North, botIntelligence);
			var bot2 = new Bot("Bot-2", 40, 20, 0, 10, location, Direction.North, botIntelligence);
			var bot3 = new Bot("Bot-3", 60, 30, 0, 15, location, Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot1);
			map.Add(bot2);
			map.Add(bot3);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			TestDelegate testDelegate = () => game.Tick();

			Assert.DoesNotThrow(testDelegate);
		}

		[Test]
		public void Tick_ForBotWhichDesireToCollectAtCellContainingBotsAndGems_ResultsInGemsCollectionByFirstBot()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Collect);
			var location = new Location(2, 4);
			var bot1 = new Bot("Bot-1", 20, 10, 0, 5, location, Direction.North, botIntelligence);
			var bot2 = new Bot("Bot-2", 40, 20, 0, 10, location, Direction.North, botIntelligence);
			var bot3 = new Bot("Bot-3", 60, 30, 0, 15, location, Direction.North, botIntelligence);
			var gem1 = new Gem(100, location);
			var gem2 = new Gem(200, location);
			var map = new Map(10, 20);
			map.Add(bot1);
			map.Add(bot2);
			map.Add(bot3);
			map.Add(gem1);
			map.Add(gem2);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(bot1.WP, Is.EqualTo(300));
			Assert.That(bot1.CollectedEntities, Is.EqualTo(new [] { gem1, gem2 }));
			Assert.That(bot1.CollectedEntities.Cast<ICollectable>().All(e => e.IsCollected), Is.True);
			Assert.That(bot2.WP, Is.EqualTo(0));
			Assert.That(bot3.WP, Is.EqualTo(0));
		}

		[Test]
		public void Tick_ForBotWhichDesireToExploreOutsideOfMap_DoNotThrow()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Explore);
			var bot = new Bot("Bot", 20, 10, 0, 5, new Location(0, 0), Direction.North, botIntelligence);
			var map = new Map(10, 20);
			map.Add(bot);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			TestDelegate testDelegate = () => game.Tick();

			Assert.DoesNotThrow(testDelegate);
		}

		[Test]
		public void Tick_ForBotWhichDesireToExploreAtCellContainingBotsAndGems_ResultsInMarkingThisCellAsExplored()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).ReturnsForAnyArgs(BotAction.Explore);
			var location1 = new Location(2, 4);
			var bot1 = new Bot("Bot-1", 20, 10, 0, 5, location1, Direction.North, botIntelligence);
			var location2 = new Location(2, 3);
			var bot2 = new Bot("Bot-2", 40, 20, 0, 10, location2, Direction.North, botIntelligence);
			var gem1 = new Gem(100, location2);
			var location3 = new Location(2, 2);
			var gem2 = new Gem(200, location3);
			var map = new Map(10, 20);
			map.Add(bot1);
			map.Add(bot2);
			map.Add(gem1);
			map.Add(gem2);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			Assert.That(map.IsExplored(bot1, location1), Is.True);
			Assert.That(map.IsExplored(bot1, location2), Is.True);
			Assert.That(map.IsExplored(bot1, location3), Is.False);
			Assert.That(map.IsExplored(bot2, location1), Is.False);
			Assert.That(map.IsExplored(bot2, location2), Is.True);
			Assert.That(map.IsExplored(bot2, location3), Is.True);
		}
	}
}
