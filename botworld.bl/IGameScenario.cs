using System.Collections.Generic;

namespace botworld.bl
{
	public interface IGameScenario
	{
		bool IsGameOver(IMap map);
		IEnumerable<IBot> GetWinners(IMap map);
	}
}
