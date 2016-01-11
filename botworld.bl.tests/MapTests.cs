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
	}
}
