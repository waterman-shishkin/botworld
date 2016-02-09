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
			scenario.GameOver.Returns(gameOverValue);
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
			scenario.Winners.Returns(bots);
			var game = new Game(null, scenario);

			Assert.That(game.Winners, Is.EqualTo(bots));
		}

		[Test]
		public void Tick_DontAffectDeadBots()
		{
			var bot = Substitute.For<IBot>();
			bot.IsDead.Returns(true);
			var bots = new [] {bot};
			var map = Substitute.For<IMap>();
			map.GetBots().Returns(bots);
			var scenario = Substitute.For<IGameScenario>();
			var game = new Game(map, scenario);

			game.Tick();

			bot.Received(0).ChooseNextAction(Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>());
		}

		// если бот умер, то него очередь уже не доходит (один бот убил другого бота)
		// TurnLeft - разворот бота
		// TurnRight - разворот бота
		// TurnStep - простое перемещение бота
		// TurnStep - попытка переместиться за границу
		// TurnStep - перемещение на клетку, куда можно встать без боя
		// TurnStep - перемещение на клетку, куда можно встать с боем
		// TurnStep - перемещение на клетку, куда можно встать с боем, но погибаем
		// TurnStep - перемещение на клетку, куда нельзя встать
		// TurnStep - перемещение на клетку, куда нельзя встать и погибаем
		// TurnStep - перемещение на клетку, куда нельзя встать, но после боя становится можно
		// TurnStep - перемещение на клетку, куда нельзя встать, но после боя становится можно, но погибаем
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
