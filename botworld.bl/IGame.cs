using System.Collections.Generic;

namespace botworld.bl
{
	public interface IGame
	{
		IMap Map { get; }
		bool GameOver { get; }
		IEnumerable<IBot> Winners { get; }
		bool Tick();
	}
}
