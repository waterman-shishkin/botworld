using System;

namespace botworld.bl
{
	public interface IKeysSequenceSource
	{
		ConsoleKey GetNextKey();
	}
}