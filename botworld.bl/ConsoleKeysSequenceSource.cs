using System;

namespace botworld.bl
{
	public class ConsoleKeysSequenceSource : IKeysSequenceSource
	{
		public ConsoleKey GetNextKey()
		{
			return Console.ReadKey().Key;
		}
	}
}