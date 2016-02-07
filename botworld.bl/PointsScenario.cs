using System.Collections.Generic;
using System.Linq;

namespace botworld.bl
{
	public class PointsScenario : IGameScenario
	{
		private readonly IMap map;
		private readonly int points;

		public bool GameOver
		{
			get
			{
				return Winners.Any() || map.GetBots().All(b => b.IsDead);
			}
		}

		public IEnumerable<IBot> Winners
		{
			get
			{
				return map.GetBots().Where(b => b.WP >= points);
			}
		}

		public PointsScenario(IMap map, int points)
		{
			this.map = map;
			this.points = points;
		}
	}
}