using System;

namespace botworld.bl
{
	public interface IKeysSequenceSource
	{
		ConsoleKeyInfo GetNextKey();
	}
}