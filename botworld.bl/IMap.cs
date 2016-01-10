using System.Collections.Generic;

namespace botworld.bl
{
	public interface IMap
	{
		int Width { get; }
		int Height { get; }
		void Add(IEntity entity);
		void Remove(IEntity entity);
		IEnumerable<IEntity> GetEntities(Location location);
		IEnumerable<IEntity> GetEntities();
		IEnumerable<IBot> GetBots();
		void MoveBot(IBot bot, Location newLocation, Direction newDirection);
		bool CanMoveBot(IBot bot, Location location);
		IEnumerable<EntityInfo> ExploreNeighborCell(IBot bot);
		Dictionary<Location, IEnumerable<EntityInfo>> GetNeighborsInfo(IBot bot);
		bool IsExplored(IBot bot, Location location);
		bool IsInRange(Location location);
	}
}
