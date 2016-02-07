using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class PointsScenarioTests
	{
		[Test]
		public void Winners_ForMapWithoutBots_ReturnsEmptyCollection()
		{
			var map = Substitute.For<IMap>();
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.Winners, Is.Empty);
		}

		[Test]
		public void Winners_ForMapWithBotsWhichAllNotCollectedNecessaryPointsToWin_ReturnsEmptyCollection()
		{
			var map = Substitute.For<IMap>();
			var bot1 = Substitute.For<IBot>();
			var bot2 = Substitute.For<IBot>();
			map.GetBots().ReturnsForAnyArgs(new[] {bot1, bot2});
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.Winners, Is.Empty);
		}

		[Test]
		public void Winners_ForMapWithSeveralBotsWhichCollectedNecessaryPointsToWin_ReturnsCollectionWithThoseBots()
		{
			var map = Substitute.For<IMap>();
			var bot1 = Substitute.For<IBot>();
			bot1.WP.Returns(150);
			var bot2 = Substitute.For<IBot>();
			bot2.WP.Returns(50);
			var bot3 = Substitute.For<IBot>();
			bot3.WP.Returns(350);
			map.GetBots().ReturnsForAnyArgs(new[] { bot1, bot2, bot3 });
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.Winners, Is.EqualTo(new[] { bot1, bot3 }));
		}

		[Test]
		public void GameOver_ForMapWithoutBots_ReturnsTrue()
		{
			var map = Substitute.For<IMap>();
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.GameOver, Is.True);
		}

		[Test]
		public void GameOver_ForMapWithBotsWhichAllAreDead_ReturnsTrue()
		{
			var map = Substitute.For<IMap>();
			var bot1 = Substitute.For<IBot>();
			bot1.IsDead.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.IsDead.Returns(true);
			map.GetBots().ReturnsForAnyArgs(new[] { bot1, bot2 });
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.GameOver, Is.True);
		}

		[Test]
		public void GameOver_ForMapWithBotsWhichNotAllAreDead_ReturnsFalse()
		{
			var map = Substitute.For<IMap>();
			var bot1 = Substitute.For<IBot>();
			bot1.IsDead.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.IsDead.Returns(false);
			var bot3 = Substitute.For<IBot>();
			bot3.IsDead.Returns(true);
			map.GetBots().ReturnsForAnyArgs(new[] { bot1, bot2, bot3 });
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.GameOver, Is.False);
		}

		[Test]
		public void GameOver_ForMapWithSeveralBotsWhichCollectedNecessaryPointsToWin_ReturnsTrue()
		{
			var map = Substitute.For<IMap>();
			var bot1 = Substitute.For<IBot>();
			bot1.WP.Returns(150);
			var bot2 = Substitute.For<IBot>();
			bot2.WP.Returns(50);
			var bot3 = Substitute.For<IBot>();
			bot3.WP.Returns(350);
			map.GetBots().ReturnsForAnyArgs(new[] { bot1, bot2, bot3 });
			var scenario = new PointsScenario(map, 100);

			Assert.That(scenario.GameOver, Is.True);
		}
	}
}
