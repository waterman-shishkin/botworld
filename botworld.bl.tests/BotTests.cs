using System;
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
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Type, Is.EqualTo(EntityType.Bot));
		}

		[Test]
		public void Direction_Returns_DirectionSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.South, null);

			Assert.That(bot.Direction, Is.EqualTo(Direction.South));
		}

		[Test]
		public void UpdateDirection_SetsPassedDirection()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateDirection(Direction.South);

			Assert.That(bot.Direction, Is.EqualTo(Direction.South));
		}

		[Test]
		public void UpdateDirection_ForNewDirection_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateDirection(Direction.South);

			botIntelligence.Received(1).OnRotation(Direction.North, Direction.South);
		}

		[Test]
		public void UpdateDirection_ForSameDirection_NotResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateDirection(Direction.North);

			botIntelligence.DidNotReceive().OnRotation(Arg.Any<Direction>(), Arg.Any<Direction>());
		}

		[Test]
		public void CollectedEntities_ForNewBot_ReturnsEmptyCollection()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.CollectedEntities, Is.Empty);
		}

		[Test]
		public void CollectedEntities_AfterCollectingSomeEntities_ReturnsCollectionOfThoseEntities()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);
			var entity1 = Substitute.For<IEntity>();
			entity1.IsCollectable.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.IsCollectable.Returns(true);

			bot.Collect(entity1);
			bot.Collect(entity2);

			Assert.That(bot.CollectedEntities, Is.EqualTo(new [] { entity1, entity2 }));
		}

		[Test]
		public void Collect_OfSomeEntity_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);
			var entity = Substitute.For<IEntity>();
			entity.IsCollectable.Returns(true);

			bot.Collect(entity);

			botIntelligence.Received(1).OnCollect(Arg.Any<EntityInfo>());
		}

		[Test]
		public void Collect_OfNonCollectableEntity_ThrowException()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);
			var entity = Substitute.For<IEntity>();
			entity.IsCollectable.Returns(false);

			var action = new TestDelegate(() => bot.Collect(entity));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void OnExplore_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);
			var info = new[] { new EntityInfo(EntityType.Gem, 1, 0, 0, 0, new Location(2, 4), true, true, 100), new EntityInfo(EntityType.Mine, 1, 100, 0, 0, new Location(2, 4), true, false, 0) };

			bot.OnExplore(info);

			botIntelligence.Received(1).OnExplore(info);
		}

		[Test]
		public void WP_ForNewBot_ReturnsZero()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.South, null);

			Assert.That(bot.WP, Is.EqualTo(0));
		}

		[Test]
		public void UpdateWP_ChangesBotWP()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			var wp = bot.UpdateWP(10);

			Assert.That(bot.WP, Is.EqualTo(10));
			Assert.That(bot.WP, Is.EqualTo(wp));
		}

		[Test]
		public void UpdateWP_ForNonZeroChange_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateWP(10);

			botIntelligence.Received(1).OnWPChange(0, 10);
		}

		[Test]
		public void UpdateWP_ForZeroChange_NotResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateWP(0);

			botIntelligence.DidNotReceive().OnWPChange(Arg.Any<int>(), Arg.Any<int>());
		}

		[Test]
		public void UpdateLocation_SetsPassedLocation()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateLocation(new Location(4, 6));

			Assert.That(bot.Location.X, Is.EqualTo(4));
			Assert.That(bot.Location.Y, Is.EqualTo(6));
		}

		[Test]
		public void UpdateLocation_ForNewLocation_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateLocation(new Location(4, 6));

			botIntelligence.Received(1).OnMove(new Location(2, 4), new Location(4, 6));
		}

		[Test]
		public void UpdateLocation_ForSameLocation_NotResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.UpdateLocation(new Location(2, 4));

			botIntelligence.DidNotReceive().OnMove(Arg.Any<Location>(), Arg.Any<Location>());
		}

		[Test]
		public void Name_Returns_NameSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Name, Is.EqualTo("Angry bot"));
		}

		[Test]
		public void Location_Returns_LocationSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.Location.X, Is.EqualTo(2));
			Assert.That(bot.Location.Y, Is.EqualTo(4));
		}

		[Test]
		public void CanShareCell_Returns_True()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.CanShareCell, Is.True);
		}

		[Test]
		public void IsCollectable_Returns_False()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.IsCollectable, Is.False);
		}

		[Test]
		public void AttackStrength_Returns_AttackStrengthSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.AttackStrength, Is.EqualTo(5));
		}

		[Test]
		public void AutoDamageStrength_Returns_AutoDamageStrengthSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 1, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.AutoDamageStrength, Is.EqualTo(1));
		}

		[Test]
		public void DefenceStrength_Returns_DefenceStrengthSetByConstructor()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			Assert.That(bot.DefenseStrength, Is.EqualTo(3));
		}

		[Test]
		public void PrepareEntityInfo_Returns_ProperInfo()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 1, 3, new Location(2, 4), Direction.North, botIntelligence);
			bot.UpdateWP(10);

			var entityInfo = bot.PrepareEntityInfo();

			Assert.That(entityInfo.Type, Is.EqualTo(EntityType.Bot));
			Assert.That(entityInfo.Location.X, Is.EqualTo(2));
			Assert.That(entityInfo.Location.Y, Is.EqualTo(4));
			Assert.That(entityInfo.CanShareCell, Is.True);
			Assert.That(entityInfo.IsCollectable, Is.False);
			Assert.That(entityInfo.HP, Is.EqualTo(100));
			Assert.That(entityInfo.AttackStrength, Is.EqualTo(5));
			Assert.That(entityInfo.AutoDamageStrength, Is.EqualTo(1));
			Assert.That(entityInfo.DefenseStrength, Is.EqualTo(3));
			Assert.That(entityInfo.WP, Is.EqualTo(10));
		}

		[Test]
		public void PrepareBotInfo_Returns_ProperInfo()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 1, 3, new Location(2, 4), Direction.North, botIntelligence);
			bot.UpdateWP(10);

			var botInfo = bot.PrepareBotInfo();

			Assert.That(botInfo.Type, Is.EqualTo(EntityType.Bot));
			Assert.That(botInfo.Location.X, Is.EqualTo(2));
			Assert.That(botInfo.Location.Y, Is.EqualTo(4));
			Assert.That(botInfo.CanShareCell, Is.True);
			Assert.That(botInfo.IsCollectable, Is.False);
			Assert.That(botInfo.HP, Is.EqualTo(100));
			Assert.That(botInfo.AttackStrength, Is.EqualTo(5));
			Assert.That(botInfo.AutoDamageStrength, Is.EqualTo(1));
			Assert.That(botInfo.DefenseStrength, Is.EqualTo(3));
			Assert.That(botInfo.WP, Is.EqualTo(10));
			Assert.That(botInfo.Direction, Is.EqualTo(Direction.North));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_ResultsNotIsDeadAndNotChangedHP()
		{
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, null);

			bot.ImpactDamage(0);

			Assert.That(bot.HP, Is.EqualTo(100));
			Assert.That(bot.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForWeakDamage_ResultsNotIsDeadAndHPDecrease()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.ImpactDamage(25);

			Assert.That(bot.HP, Is.EqualTo(75));
			Assert.That(bot.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForStrongDamage_ResultsIsDeadAndZeroHP()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.ImpactDamage(250);

			Assert.That(bot.HP, Is.EqualTo(0));
			Assert.That(bot.IsDead, Is.True);
		}

		[Test]
		public void ImpactDamage_ForNotZeroDamage_ResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.ImpactDamage(10);

			botIntelligence.Received(1).OnDamage(100, 90);
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_NotResultsInBotIntelligenceNotification()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			bot.ImpactDamage(0);

			botIntelligence.DidNotReceive().OnDamage(Arg.Any<double>(), Arg.Any<double>());
		}

		[Test]
		public void ChooseNextAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseNextAction(Arg.Any<BotInfo>(), Arg.Any<Dictionary<Location, IEnumerable<EntityInfo>>>()).Returns(BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None);
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			var expectedActions = new[] { BotAction.TurnLeft, BotAction.Step, BotAction.TurnRight, BotAction.Act, BotAction.Explore, BotAction.Collect, BotAction.None };
			var actions = new List<BotAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseNextAction(null));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}

		[Test]
		public void ChooseInvasionResponseAction_Always_ReturnsActionsReturnedByBotIntelligence()
		{
			var botIntelligence = Substitute.For<IBotIntelligence>();
			botIntelligence.ChooseInvasionResponseAction(Arg.Any<BotInfo>(), Arg.Any<EntityInfo>()).Returns(InvasionResponseAction.None, InvasionResponseAction.Attack);
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

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
			var bot = new Bot("Angry bot", 100, 5, 0, 3, new Location(2, 4), Direction.North, botIntelligence);

			var expectedActions = new[] { AttackResponseAction.None, AttackResponseAction.Attack };
			var actions = new List<AttackResponseAction>();
			for (var i = 0; i < expectedActions.Length; i++)
				actions.Add(bot.ChooseAttackResponseAction(Substitute.For<IEntity>()));

			Assert.That(actions.ToArray(), Is.EqualTo(expectedActions));
		}
	}
}
