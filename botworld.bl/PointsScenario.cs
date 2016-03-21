using System.Collections.Generic;
using System.Linq;

namespace botworld.bl
{
	public class PointsScenario : IGameScenario
	{
		public int WP { get; private set; }

		public bool IsGameOver(IMap map)
		{
			return GetWinners(map).Any() || map.GetBots().All(b => b.IsDead);
		}

		public IEnumerable<IBot> GetWinners(IMap map)
		{
			return map.GetBots().Where(b => b.WP >= WP);
		}

		public PointsScenario(int wp)
		{
			WP = wp;
		}
	}
}