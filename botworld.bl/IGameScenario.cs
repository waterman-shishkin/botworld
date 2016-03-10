using System.Collections.Generic;

namespace botworld.bl
{
	public interface IGameScenario
	{
		bool GameOver { get; }
		IEnumerable<IBot> Winners { get; }
	}
}
