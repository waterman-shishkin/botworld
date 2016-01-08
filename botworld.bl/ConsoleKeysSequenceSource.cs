using System;

namespace botworld.bl
{
	public class ConsoleKeysSequenceSource : IKeysSequenceSource
	{
		public ConsoleKeyInfo GetNextKey()
		{
			return Console.ReadKey();
		}
	}
}