using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class MapTests
	{
		[Test]
		public void Width_Returns_WidthSetByConstructor()
		{
			var map = new Map(10, 20);

			Assert.That(map.Width, Is.EqualTo(10));
		}

		[Test]
		public void Height_Returns_WidthSetByConstructor()
		{
			var map = new Map(10, 20);

			Assert.That(map.Height, Is.EqualTo(20));
		}

		[Test]
		public void Add_EntityWithLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(new Location(2, 40));

			var action = new TestDelegate(() => map.Add(entity));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void Add_BotWithLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 40));

			var action = new TestDelegate(() => map.Add(bot));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void Add_Entity_PlacesItInSpecifiedLocation()
		{
			var map = new Map(10, 20);
			var entity = Substitute.For<IEntity>();
			var location = new Location(2,4);
			entity.Location.Returns(location);

			map.Add(entity);

			Assert.That(map.GetEntities(location).Single(), Is.EqualTo(entity));
		}

		[Test]
		public void Add_Bot_PlacesItInSpecifiedLocation()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2,4);
			bot.Location.Returns(location);

			map.Add(bot);

			Assert.That(map.GetEntities(location).Single(), Is.EqualTo(bot));
		}

		[Test]
		public void Add_FriendlyEntityInPlaceOccupiedByFriendlyEntity_PlacesItInSpecifiedLocation()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(location);
			entity1.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(location);
			entity2.CanShareCell.Returns(true);

			map.Add(entity1);
			map.Add(entity2);

			Assert.That(map.GetEntities(location), Is.EqualTo(new [] { entity1, entity2 }));
		}

		[Test]
		public void Add_FriendlyEntityInPlaceOccupiedByNotFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(location);
			entity1.CanShareCell.Returns(false);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(location);
			entity2.CanShareCell.Returns(true);

			map.Add(entity1);
			var action = new TestDelegate(() => map.Add(entity2));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Add_NotFriendlyEntityInPlaceOccupiedByFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(location);
			entity1.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(location);
			entity2.CanShareCell.Returns(false);

			map.Add(entity1);
			var action = new TestDelegate(() => map.Add(entity2));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Add_FriendlyBotInPlaceOccupiedByFriendlyEntity_PlacesItInSpecifiedLocation()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(true);

			map.Add(entity);
			map.Add(bot);

			Assert.That(map.GetEntities(location), Is.EqualTo(new [] { entity, bot }));
		}

		[Test]
		public void Add_FriendlyBotInPlaceOccupiedByNotFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(false);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(true);

			map.Add(entity);
			var action = new TestDelegate(() => map.Add(bot));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Add_NotFriendlyBotInPlaceOccupiedByFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(false);

			map.Add(entity);
			var action = new TestDelegate(() => map.Add(bot));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Remove_EntityNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(new Location(2, 4));

			var action = new TestDelegate(() => map.Remove(entity));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Remove_BotNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));

			var action = new TestDelegate(() => map.Remove(bot));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void Remove_Entity_RemovesEntityFromMap()
		{
			var map = new Map(10, 20);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(new Location(2, 4));

			map.Add(entity);
			map.Remove(entity);

			Assert.That(map.GetEntities().Count(), Is.EqualTo(0));
		}

		[Test]
		public void Remove_Bot_RemovesBotFromMap()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));

			map.Add(bot);
			map.Remove(bot);

			Assert.That(map.GetEntities().Count(), Is.EqualTo(0));
		}

		[Test]
		public void Remove_EntityOneOfSeveralInLocation_RemovesEntityFromMap()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(location);
			entity1.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(location);
			entity2.CanShareCell.Returns(true);

			map.Add(entity1);
			map.Add(entity2);
			map.Remove(entity1);

			Assert.That(map.GetEntities(location).Single(), Is.EqualTo(entity2));
		}

		[Test]
		public void Remove_BotOneOfSeveralInLocation_RemovesBotFromMap()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var bot1 = Substitute.For<IBot>();
			bot1.Location.Returns(location);
			bot1.CanShareCell.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.Location.Returns(location);
			bot2.CanShareCell.Returns(true);

			map.Add(bot1);
			map.Add(bot2);
			map.Remove(bot1);

			Assert.That(map.GetEntities(location).Single(), Is.EqualTo(bot2));
		}

		[Test]
		public void GetEntities_ForLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);

			var action = new TestDelegate(() => map.GetEntities(new Location(2, 40)));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void GetEntities_ForEmptyLocation_ReturnsEmptyCollection()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);

			var entities = map.GetEntities(location);

			Assert.That(entities.Count(), Is.EqualTo(0));
		}

		[Test]
		public void GetEntities_ForOccupiedLocation_ReturnsExpectedCollection()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var bot1 = Substitute.For<IBot>();
			bot1.Location.Returns(location);
			bot1.CanShareCell.Returns(true);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(location);
			entity1.CanShareCell.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.Location.Returns(location);
			bot2.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(location);
			entity2.CanShareCell.Returns(true);

			map.Add(bot1);
			map.Add(entity1);
			map.Add(bot2);
			map.Add(entity2);

			var entities = map.GetEntities(location);

			var expectedEntities = new[] {entity1, bot1, entity2, bot2};
			Assert.That(entities.All(expectedEntities.Contains) && expectedEntities.Length == entities.Count(), Is.True);
		}

		[Test]
		public void GetEntities_ForEmptyMap_ReturnsEmptyCollection()
		{
			var map = new Map(10, 20);

			var entities = map.GetEntities();

			Assert.That(entities.Count(), Is.EqualTo(0));
		}

		[Test]
		public void GetEntities_ForOccupiedMap_ReturnsExpectedCollection()
		{
			var map = new Map(10, 20);
			var bot1 = Substitute.For<IBot>();
			bot1.Location.Returns(new Location(1, 2));
			bot1.CanShareCell.Returns(true);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(new Location(2, 4));
			entity1.CanShareCell.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.Location.Returns(new Location(6, 8));
			bot2.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(new Location(5, 5));
			entity2.CanShareCell.Returns(true);

			map.Add(bot1);
			map.Add(entity1);
			map.Add(bot2);
			map.Add(entity2);

			var entities = map.GetEntities();

			var expectedEntities = new[] {entity1, bot1, entity2, bot2};
			Assert.That(entities.All(expectedEntities.Contains) && expectedEntities.Length == entities.Count(), Is.True);
		}

		[Test]
		public void GetBots_ForEmptyMap_ReturnsEmptyCollection()
		{
			var map = new Map(10, 20);

			var bots = map.GetBots();

			Assert.That(bots.Count(), Is.EqualTo(0));
		}

		[Test]
		public void GetBots_ForMapOccupiedNotByBots_ReturnsEmptyCollection()
		{
			var map = new Map(10, 20);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(new Location(2, 4));
			entity1.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(new Location(5, 5));
			entity2.CanShareCell.Returns(true);

			map.Add(entity1);
			map.Add(entity2);

			var bots = map.GetBots();

			Assert.That(bots.Count(), Is.EqualTo(0));
		}

		[Test]
		public void GetBots_ForMapOccupiedByBotsAndOtherEntities_ReturnsExpectedCollection()
		{
			var map = new Map(10, 20);
			var bot1 = Substitute.For<IBot>();
			bot1.Location.Returns(new Location(1, 2));
			bot1.CanShareCell.Returns(true);
			var entity1 = Substitute.For<IEntity>();
			entity1.Location.Returns(new Location(2, 4));
			entity1.CanShareCell.Returns(true);
			var bot2 = Substitute.For<IBot>();
			bot2.Location.Returns(new Location(6, 8));
			bot2.CanShareCell.Returns(true);
			var entity2 = Substitute.For<IEntity>();
			entity2.Location.Returns(new Location(5, 5));
			entity2.CanShareCell.Returns(true);

			map.Add(bot1);
			map.Add(entity1);
			map.Add(bot2);
			map.Add(entity2);

			var bots = map.GetBots();

			var expectedBots = new[] {bot1, bot2};
			Assert.That(bots.All(expectedBots.Contains) && expectedBots.Length == bots.Count(), Is.True);
		}

		[Test]
		public void MoveBot_ForBotNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(6, 8));

			var action = new TestDelegate(() => map.MoveBot(bot, new Location(2, 4), Direction.North));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void MoveBot_ToLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			map.Add(bot);

			var action = new TestDelegate(() => map.MoveBot(bot, new Location(-1, 4), Direction.North));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void MoveBot_WithNewLocationAndDirection_CausesBotToUpdateLocationAndDirection()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			bot.Direction.Returns(Direction.North);
			map.Add(bot);

			var location = new Location(4, 6);
			var direction = Direction.South;
			map.MoveBot(bot, location, direction);

			bot.Received(1).UpdateLocation(location);
			bot.Received(1).UpdateDirection(direction);
		}

		[Test]
		public void MoveBot_WithNewLocationAndOldDirection_CausesBotToUpdateLocationOnly()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			var direction = Direction.North;
			bot.Direction.Returns(direction);
			map.Add(bot);

			var location = new Location(4, 6);
			map.MoveBot(bot, location, direction);

			bot.Received(1).UpdateLocation(location);
			bot.DidNotReceive().UpdateDirection(Arg.Any<Direction>());
		}

		[Test]
		public void MoveBot_WithOldLocationAndNewDirection_CausesBotToUpdateDirectionOnly()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			bot.Direction.Returns(Direction.North);
			map.Add(bot);

			var direction = Direction.South;
			map.MoveBot(bot, location, direction);

			bot.DidNotReceive().UpdateLocation(Arg.Any<Location>());
			bot.Received(1).UpdateDirection(direction);
		}

		[Test]
		public void MoveBot_WithOldLocationAndOldDirection_NotCausesBotToUpdateLocationOrDirection()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			var direction = Direction.North;
			bot.Direction.Returns(direction);
			map.Add(bot);

			map.MoveBot(bot, location, direction);

			bot.DidNotReceive().UpdateLocation(Arg.Any<Location>());
			bot.DidNotReceive().UpdateDirection(Arg.Any<Direction>());
		}

		[Test]
		public void MoveBot_ForFriendlyBotAndLocationOccupiedByFriendlyEntity_CausesBotToUpdateLocation()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(true);
			map.Add(entity);
			map.Add(bot);

			map.MoveBot(bot, location, Direction.North);

			bot.Received(1).UpdateLocation(location);
		}

		[Test]
		public void MoveBot_ForFriendlyBotAndLocationOccupiedByNotFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(false);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(true);
			map.Add(entity);
			map.Add(bot);

			var action = new TestDelegate(() => map.MoveBot(bot, location, Direction.North));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void MoveBot_ForNotFriendlyBotAndLocationOccupiedByFriendlyEntity_ThrowException()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(false);
			map.Add(entity);
			map.Add(bot);

			var action = new TestDelegate(() => map.MoveBot(bot, location, Direction.North));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void CanMoveBot_ForBotNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(6, 8));

			var action = new TestDelegate(() => map.CanMoveBot(bot, new Location(2, 4)));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void CanMoveBot_ForLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			map.Add(bot);

			var action = new TestDelegate(() => map.CanMoveBot(bot, new Location(-1, 4)));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void CanMoveBot_WithSameLocation_ReturnsTrue()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			map.Add(bot);

			var canMove = map.CanMoveBot(bot, location);

			Assert.That(canMove, Is.True);
		}

		[Test]
		public void CanMoveBot_ForFriendlyBotAndLocationOccupiedByFriendlyEntity_ReturnsTrue()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(true);
			map.Add(entity);
			map.Add(bot);

			var canMove = map.CanMoveBot(bot, location);

			Assert.That(canMove, Is.True);
		}

		[Test]
		public void CanMoveBot_ForFriendlyBotAndLocationOccupiedByNotFriendlyEntity_ReturnsFalse()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(false);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(true);
			map.Add(entity);
			map.Add(bot);

			var canMove = map.CanMoveBot(bot, location);

			Assert.That(canMove, Is.False);
		}

		[Test]
		public void CanMoveBot_ForNotFriendlyBotAndLocationOccupiedByFriendlyEntity_ReturnsFalse()
		{
			var map = new Map(10, 20);
			var location = new Location(2, 4);
			var entity = Substitute.For<IEntity>();
			entity.Location.Returns(location);
			entity.CanShareCell.Returns(true);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(4, 6));
			bot.CanShareCell.Returns(false);
			map.Add(entity);
			map.Add(bot);

			var canMove = map.CanMoveBot(bot, location);

			Assert.That(canMove, Is.False);
		}
	}
}
