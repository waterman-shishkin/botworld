using System;
using System.Collections.Generic;
using System.Linq;

namespace botworld.bl
{
	public class Map : IMap
	{
		private readonly Cell[][] cells;
		private readonly List<IBot> bots = new List<IBot>();
		private int botNextId;
		private readonly Dictionary<IBot, int> botIds = new Dictionary<IBot, int>();

		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			cells = new Cell[Width][];
			for (var i = 0; i < Width; i++)
				cells[i] = new Cell[Height];
		}

		public int Width { get; private set; }

		public int Height { get; private set; }

		public void Add(IEntity entity)
		{
			var location = entity.Location;
			if (!CanPlaceEntity(entity, location))
				throw new InvalidOperationException("It is forbidden to place several entities in same place if any of them can not share cell");

			var bot = entity as IBot;
			if (bot != null)
			{
				bots.Add(bot);
				botIds.Add(bot, botNextId++);
				ExploreCell(bot);
			}
			else
				GetCell(location).Entities.Add(entity);
		}

		private Cell GetCell(Location location)
		{
			var cell = cells[location.X][location.Y];
			if (cell == null)
				cells[location.X][location.Y] = cell = new Cell();
			return cell;
		}

		public void Remove(IEntity entity)
		{
			var bot = entity as IBot;
			if (bot != null)
				bots.Remove(bot);
			else
				GetCell(entity.Location).Entities.Remove(entity);
		}

		public IEnumerable<IEntity> GetEntities(Location location)
		{
			return GetCell(location).Entities.Concat(bots.Where(b => b.Location == location));
		}

		public IEnumerable<IEntity> GetEntities()
		{
			return cells.Aggregate((IEnumerable<IEntity>)bots, (aggregate, array) => aggregate.Concat(array.Where(cell => cell != null).SelectMany(cell => cell.Entities)));
		}

		public IEnumerable<IBot> GetBots()
		{
			return bots;
		}

		public void MoveBot(IBot bot, Location newLocation, Direction newDirection)
		{
			var location = bot.Location;
			if (location != newLocation)
			{
				if (!CanMoveBot(bot, newLocation))
					throw new InvalidOperationException("It is impossible to move bot to location passed");
				ExploreCell(bot);
				bot.UpdateLocation(newLocation);
			}
			var direction = bot.Direction;
			if (direction != newDirection)
				bot.UpdateDirection(newDirection);
		}

		public bool CanMoveBot(IBot bot, Location location)
		{
			return CanPlaceEntity(bot, location);
		}

		private bool CanPlaceEntity(IEntity entity, Location location)
		{
			var entities = GetEntities(location).ToArray();
			return entities.All(e => e.CanShareCell) && (!entities.Any() || entity.CanShareCell);
		}

		public IEnumerable<EntityInfo> ExploreNeighborCell(IBot bot)
		{
			var neighborLocation = bot.Location.GetNeighborLocationInDirection(bot.Direction);
			return ExploreCell(bot, neighborLocation);
		}

		private IEnumerable<EntityInfo> ExploreCell(IBot bot)
		{
			var location = bot.Location;
			return ExploreCell(bot, location);
		}

		private IEnumerable<EntityInfo> ExploreCell(IBot bot, Location location)
		{
			GetCell(location).VisitorsIds.Add(botIds[bot]);
			return GetEntitiesInfo(location);
		}

		private IEnumerable<EntityInfo> GetEntitiesInfo(Location location)
		{
			return GetEntities(location).Select(e => e.PrepareEntityInfo());
		}

		public Dictionary<Location, IEnumerable<EntityInfo>> GetNeighborsInfo(IBot bot)
		{
			var result = new Dictionary<Location, IEnumerable<EntityInfo>> { { bot.Location, GetEntitiesInfo(bot.Location) } };
			var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
			foreach (var direction in directions)
			{
				var location = bot.Location.GetNeighborLocationInDirection(direction);
				if (IsInRange(location) && IsExplored(bot, location))
					result.Add(location, GetEntitiesInfo(location));
			}
			return result;
		}

		public bool IsExplored(IBot bot, Location location)
		{
			return GetCell(location).VisitorsIds.Contains(botIds[bot]);
		}

		public bool IsInRange(Location location)
		{
			return location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Width;
		}
	}
}