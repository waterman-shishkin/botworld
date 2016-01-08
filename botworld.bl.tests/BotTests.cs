using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class BotTests
	{
		[Test]
		public void Type_Returns_Bot()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Type, Is.EqualTo(EntityType.Bot));
		}

		[Test]
		public void Direction_Returns_DirectionSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.South, null);

			Assert.That(bot.Direction, Is.EqualTo(Direction.South));
		}

		[Test]
		public void UpdateDirection_SetsPassedDirection()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			bot.UpdateDirection(Direction.South);

			Assert.That(bot.Direction, Is.EqualTo(Direction.South));
		}

		[Test]
		public void Name_Returns_NameSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Name, Is.EqualTo("Angry bot"));
		}

		[Test]
		public void Location_Returns_LocationSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Location.X, Is.EqualTo(2));
			Assert.That(bot.Location.Y, Is.EqualTo(4));
		}

		[Test]
		public void CanShareCell_Returns_True()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.CanShareCell, Is.True);
		}

		[Test]
		public void IsCollectable_Returns_False()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.IsCollectable, Is.False);
		}

		[Test]
		public void AttackStrength_Returns_AttackStrengthSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.AttackStrength, Is.EqualTo(5));
		}

		[Test]
		public void DefenceStrength_Returns_DefenceStrengthSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.DefenceStrength, Is.EqualTo(3));
		}

		[Test]
		public void PrepareEntityInfo_Returns_ProperInfo()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			var entityInfo = bot.PrepareEntityInfo();

			Assert.That(entityInfo.Type, Is.EqualTo(EntityType.Bot));
			Assert.That(entityInfo.Location.X, Is.EqualTo(2));
			Assert.That(entityInfo.Location.Y, Is.EqualTo(4));
			Assert.That(entityInfo.CanShareCell, Is.True);
			Assert.That(entityInfo.IsCollectable, Is.False);
			Assert.That(entityInfo.HP, Is.EqualTo(100));
			Assert.That(entityInfo.AttackStrength, Is.EqualTo(5));
			Assert.That(entityInfo.DefenceStrength, Is.EqualTo(3));
		}

		[Test]
		public void PrepareBotInfo_Returns_ProperInfo()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			var botInfo = bot.PrepareBotInfo();

			Assert.That(botInfo.Type, Is.EqualTo(EntityType.Bot));
			Assert.That(botInfo.Location.X, Is.EqualTo(2));
			Assert.That(botInfo.Location.Y, Is.EqualTo(4));
			Assert.That(botInfo.CanShareCell, Is.True);
			Assert.That(botInfo.IsCollectable, Is.False);
			Assert.That(botInfo.HP, Is.EqualTo(100));
			Assert.That(botInfo.AttackStrength, Is.EqualTo(5));
			Assert.That(botInfo.DefenceStrength, Is.EqualTo(3));
			Assert.That(botInfo.Direction, Is.EqualTo(Direction.North));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_ResultsNotIsDeadAndNotChangedHP()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			bot.ImpactDamage(0);

			Assert.That(bot.HP, Is.EqualTo(100));
			Assert.That(bot.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForWeakDamage_ResultsNotIsDeadAndHPDecrease()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			bot.ImpactDamage(25);

			Assert.That(bot.HP, Is.EqualTo(75));
			Assert.That(bot.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForStrongDamage_ResultsIsDeadAndZeroHP()
		{
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, null);

			bot.ImpactDamage(250);

			Assert.That(bot.HP, Is.EqualTo(0));
			Assert.That(bot.IsDead, Is.True);
		}

		[Test]
		public void ChooseNextAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>()).Returns(BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None);
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, botIntelligence);

			var expectedActions = new[] { BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None };
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseNextAction());

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseInvasionResponseAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).Returns(InvasionResponseAction.None, InvasionResponseAction.Attack);
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, botIntelligence);

			var expectedActions = new[] { InvasionResponseAction.None, InvasionResponseAction.Attack };
			var actions = new List<InvasionResponseAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseInvasionResponseAction(Substitute.For<IEntity>()));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseAttackResponseAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseAttackResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).Returns(AttackResponseAction.None, AttackResponseAction.Attack);
			var bot = new Bot("Angry bot", 100, 5, 3, new Location(2, 4), Direction.North, botIntelligence);

			var expectedActions = new[] { AttackResponseAction.None, AttackResponseAction.Attack };
			var actions = new List<AttackResponseAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseAttackResponseAction(Substitute.For<IEntity>()));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
