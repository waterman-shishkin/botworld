using System;
using System.Collections.Generic;
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

		[Test]
		public void IsInRange_ForLocationInMapRange_ReturnsTrue()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(2, 4));

			Assert.That(isInRange, Is.True);
		}

		[Test]
		public void IsInRange_ForLocationAtNorthMapBorder_ReturnsTrue()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(2, 0));

			Assert.That(isInRange, Is.True);
		}

		[Test]
		public void IsInRange_ForLocationAtSouthMapBorder_ReturnsTrue()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(2, 19));

			Assert.That(isInRange, Is.True);
		}

		[Test]
		public void IsInRange_ForLocationAtWestMapBorder_ReturnsTrue()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(0, 4));

			Assert.That(isInRange, Is.True);
		}

		[Test]
		public void IsInRange_ForLocationAtEastMapBorder_ReturnsTrue()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(9, 4));

			Assert.That(isInRange, Is.True);
		}

		[Test]
		public void IsInRange_ForLocationBehindNorthMapBorder_ReturnsFalse()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(2, -1));

			Assert.That(isInRange, Is.False);
		}

		[Test]
		public void IsInRange_ForLocationBehindSouthMapBorder_ReturnsFalse()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(2, 20));

			Assert.That(isInRange, Is.False);
		}

		[Test]
		public void IsInRange_ForLocationBehindWestMapBorder_ReturnsFalse()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(-1, 4));

			Assert.That(isInRange, Is.False);
		}

		[Test]
		public void IsInRange_ForLocationBehindEastMapBorder_ReturnsFalse()
		{
			var map = new Map(10, 20);

			var isInRange = map.IsInRange(new Location(10, 4));

			Assert.That(isInRange, Is.False);
		}

		[Test]
		public void IsExplored_ForBotNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);

			var action = new TestDelegate(() => map.IsExplored(bot, location));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void IsExplored_ToLocationOutOfRange_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			map.Add(bot);

			var action = new TestDelegate(() => map.IsExplored(bot, new Location(-1, 4)));

			Assert.Throws<IndexOutOfRangeException>(action);
		}

		[Test]
		public void IsExplored_ForNeverOcuupiedLocation_ReturnsFalse()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			bot.Location.Returns(new Location(2, 4));
			map.Add(bot);

			var isExplored = map.IsExplored(bot, new Location(1, 4));

			Assert.That(isExplored, Is.False);
		}

		[Test]
		public void IsExplored_ForBotSeedLocation_ReturnsTrue()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			map.Add(bot);

			var isExplored = map.IsExplored(bot, location);

			Assert.That(isExplored, Is.True);
		}

		[Test]
		public void IsExplored_AfterBotMoveForOldAndNewLocations_ReturnsTrue()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var oldLocation = new Location(2, 4);
			bot.Location.Returns(oldLocation);
			map.Add(bot);
			var newLocation = new Location(3, 4);
			map.MoveBot(bot, newLocation, bot.Direction);

			var isOldExplored = map.IsExplored(bot, oldLocation);
			var isNewExplored = map.IsExplored(bot, newLocation);

			Assert.That(isOldExplored, Is.True);
			Assert.That(isNewExplored, Is.True);
		}

		[Test]
		public void IsExplored_ForExploredLocation_ReturnsTrue()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			bot.Direction.Returns(Direction.North);
			map.Add(bot);
			map.ExploreNeighborCell(bot);

			var isExplored = map.IsExplored(bot, new Location(2, 3));

			Assert.That(isExplored, Is.True);
		}

		[Test]
		public void IsExplored_ForCellExploredByOtherBot_ReturnsFalse()
		{
			var map = new Map(10, 20);
			var bot1 = Substitute.For<IBot>();
			var location1 = new Location(2, 4);
			bot1.Location.Returns(location1);
			map.Add(bot1);
			var bot2 = Substitute.For<IBot>();
			var location2 = new Location(4, 6);
			bot2.Location.Returns(location2);
			map.Add(bot2);

			var isExplored = map.IsExplored(bot1, location2);

			Assert.That(isExplored, Is.False);
		}

		[Test]
		public void GetNeighborsInfo_ForBotNotHostedByMap_ThrowException()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);

			var action = new TestDelegate(() => map.GetNeighborsInfo(bot));

			Assert.Throws<InvalidOperationException>(action);
		}

		[Test]
		public void GetNeighborsInfo_ForBotExploredOnlyItsLocation_ReturnsInfoOnlyForThisLocationWhichIsEmpty()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			map.Add(bot);

			var info = map.GetNeighborsInfo(bot);

			Assert.That(info.Keys.Single(), Is.EqualTo(location));
			Assert.That(info.Values.Single().Count(), Is.EqualTo(0));
		}

		[Test]
		public void GetNeighborsInfo_ForBotExploredSomeCellsAroundItsLocation_ReturnsInfoOnlyForTheseLocations()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(2, 4);
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(true);
			map.Add(bot);
			var locations = new[] {location, new Location(1, 4), new Location(2, 5), new Location(2, 3)};
			var expectedInfos = new [] {new List<EntityInfo> {new EntityInfo(EntityType.Gem, 10, 10, 10, locations[0], true, true), new BotInfo(10, 10, 10, locations[0], Direction.North )}, 
										new List<EntityInfo> {new EntityInfo(EntityType.Mine, 20, 20, 20, locations[1], true, false), new BotInfo(20, 20, 20, locations[1], Direction.South)}, 
										new List<EntityInfo> {new EntityInfo(EntityType.Wall, 30, 30, 20, locations[2], false, false)}, 
										new List<EntityInfo>()};

			foreach (var entity in expectedInfos.SelectMany(l => l).Select(CreateEntity))
				map.Add(entity);

			bot.Direction.Returns(Direction.West);
			map.ExploreNeighborCell(bot);
			bot.Direction.Returns(Direction.South);
			map.ExploreNeighborCell(bot);
			bot.Direction.Returns(Direction.North);
			map.ExploreNeighborCell(bot);
			
			var info = map.GetNeighborsInfo(bot);

			Assert.That(info.Keys.Count(), Is.EqualTo(locations.Length));
			for (var i = 0; i < locations.Length; i++)
			{
				var expectedLocation = locations[i];
				var locationInfo = info[expectedLocation].ToArray();
				Assert.That(locationInfo.Length, Is.EqualTo(expectedInfos[i].Count));
				for (var j = 0; j < locationInfo.Length; j++)
				{
					var entityInfo = locationInfo[j];
					var expectedInfo = expectedInfos[i][j];
					Assert.That(entityInfo.Type, Is.EqualTo(expectedInfo.Type));
					Assert.That(entityInfo.HP, Is.EqualTo(expectedInfo.HP));
					Assert.That(entityInfo.AttackStrength, Is.EqualTo(expectedInfo.AttackStrength));
					Assert.That(entityInfo.DefenceStrength, Is.EqualTo(expectedInfo.DefenceStrength));
					Assert.That(entityInfo.Location, Is.EqualTo(expectedInfo.Location));
					Assert.That(entityInfo.CanShareCell, Is.EqualTo(expectedInfo.CanShareCell));
					Assert.That(entityInfo.IsCollectable, Is.EqualTo(expectedInfo.IsCollectable));
					if (entityInfo.Type == EntityType.Bot)
						Assert.That(((BotInfo)entityInfo).Direction, Is.EqualTo(((BotInfo)expectedInfo).Direction));
				}
			}
		}

		[Test]
		public void GetNeighborsInfo_ForBotLocatedAtNorthWestBoundaries_ReturnsInfoForExploredLocations()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(0, 0);
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(true);
			map.Add(bot);
			var locations = new[] {location, new Location(1, 0), new Location(0, 1)};
			var expectedInfos = new [] {new List<EntityInfo> {new EntityInfo(EntityType.Gem, 10, 10, 10, locations[0], true, true), new BotInfo(10, 10, 10, locations[0], Direction.North )}, 
										new List<EntityInfo> {new EntityInfo(EntityType.Mine, 20, 20, 20, locations[1], true, false)}, 
										new List<EntityInfo>()};

			foreach (var entity in expectedInfos.SelectMany(l => l).Select(CreateEntity))
				map.Add(entity);

			bot.Direction.Returns(Direction.East);
			map.ExploreNeighborCell(bot);
			bot.Direction.Returns(Direction.South);
			map.ExploreNeighborCell(bot);
			
			var info = map.GetNeighborsInfo(bot);

			Assert.That(info.Keys.Count(), Is.EqualTo(locations.Length));
			for (var i = 0; i < locations.Length; i++)
			{
				var expectedLocation = locations[i];
				var locationInfo = info[expectedLocation].ToArray();
				Assert.That(locationInfo.Length, Is.EqualTo(expectedInfos[i].Count));
				for (var j = 0; j < locationInfo.Length; j++)
				{
					var entityInfo = locationInfo[j];
					var expectedInfo = expectedInfos[i][j];
					Assert.That(entityInfo.Type, Is.EqualTo(expectedInfo.Type));
					Assert.That(entityInfo.HP, Is.EqualTo(expectedInfo.HP));
					Assert.That(entityInfo.AttackStrength, Is.EqualTo(expectedInfo.AttackStrength));
					Assert.That(entityInfo.DefenceStrength, Is.EqualTo(expectedInfo.DefenceStrength));
					Assert.That(entityInfo.Location, Is.EqualTo(expectedInfo.Location));
					Assert.That(entityInfo.CanShareCell, Is.EqualTo(expectedInfo.CanShareCell));
					Assert.That(entityInfo.IsCollectable, Is.EqualTo(expectedInfo.IsCollectable));
					if (entityInfo.Type == EntityType.Bot)
						Assert.That(((BotInfo)entityInfo).Direction, Is.EqualTo(((BotInfo)expectedInfo).Direction));
				}
			}
		}

		[Test]
		public void GetNeighborsInfo_ForBotLocatedAtSouthEastBoundaries_ReturnsInfoForExploredLocations()
		{
			var map = new Map(10, 20);
			var bot = Substitute.For<IBot>();
			var location = new Location(9, 19);
			bot.Location.Returns(location);
			bot.CanShareCell.Returns(true);
			map.Add(bot);
			var locations = new[] {location, new Location(8, 19), new Location(9, 18)};
			var expectedInfos = new [] {new List<EntityInfo> {new EntityInfo(EntityType.Gem, 10, 10, 10, locations[0], true, true), new BotInfo(10, 10, 10, locations[0], Direction.North )}, 
										new List<EntityInfo> {new EntityInfo(EntityType.Mine, 20, 20, 20, locations[1], true, false)}, 
										new List<EntityInfo>()};

			foreach (var entity in expectedInfos.SelectMany(l => l).Select(CreateEntity))
				map.Add(entity);

			bot.Direction.Returns(Direction.West);
			map.ExploreNeighborCell(bot);
			bot.Direction.Returns(Direction.North);
			map.ExploreNeighborCell(bot);
			
			var info = map.GetNeighborsInfo(bot);

			Assert.That(info.Keys.Count(), Is.EqualTo(locations.Length));
			for (var i = 0; i < locations.Length; i++)
			{
				var expectedLocation = locations[i];
				var locationInfo = info[expectedLocation].ToArray();
				Assert.That(locationInfo.Length, Is.EqualTo(expectedInfos[i].Count));
				for (var j = 0; j < locationInfo.Length; j++)
				{
					var entityInfo = locationInfo[j];
					var expectedInfo = expectedInfos[i][j];
					Assert.That(entityInfo.Type, Is.EqualTo(expectedInfo.Type));
					Assert.That(entityInfo.HP, Is.EqualTo(expectedInfo.HP));
					Assert.That(entityInfo.AttackStrength, Is.EqualTo(expectedInfo.AttackStrength));
					Assert.That(entityInfo.DefenceStrength, Is.EqualTo(expectedInfo.DefenceStrength));
					Assert.That(entityInfo.Location, Is.EqualTo(expectedInfo.Location));
					Assert.That(entityInfo.CanShareCell, Is.EqualTo(expectedInfo.CanShareCell));
					Assert.That(entityInfo.IsCollectable, Is.EqualTo(expectedInfo.IsCollectable));
					if (entityInfo.Type == EntityType.Bot)
						Assert.That(((BotInfo)entityInfo).Direction, Is.EqualTo(((BotInfo)expectedInfo).Direction));
				}
			}
		}

		private static IEntity CreateEntity(EntityInfo entityInfo)
		{
			var entity = entityInfo.Type == EntityType.Bot ? Substitute.For<IBot>() : Substitute.For<IEntity>();
			entity.Type.Returns(entityInfo.Type);
			entity.HP.Returns(entityInfo.HP);
			entity.AttackStrength.Returns(entityInfo.AttackStrength);
			entity.DefenceStrength.Returns(entityInfo.DefenceStrength);
			entity.Location.Returns(entityInfo.Location);
			entity.CanShareCell.Returns(entityInfo.CanShareCell);
			entity.IsCollectable.Returns(entityInfo.IsCollectable);
			if (entityInfo.Type == EntityType.Bot)
				((IBot)entity).Direction.Returns(((BotInfo)entityInfo).Direction);
			return entity;
		}

		//ExploreNeighborCell
		//свой-чужой
		//заграница
		//пометка исследованных
		//информация о клетке
		//боты и не боты
		//много всяких
		//границы
	}
}
