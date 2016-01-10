using System.Collections.Generic;

namespace botworld.bl
{
	public class Cell
	{
		private List<IEntity> entities;
		private HashSet<int> visitorsIds;

		public List<IEntity> Entities
		{
			get
			{
				return entities ?? (entities = new List<IEntity>());
			}
		}

		public HashSet<int> VisitorsIds
		{
			get
			{
				return visitorsIds ?? (visitorsIds = new HashSet<int>());
			}
		}
	}
}
