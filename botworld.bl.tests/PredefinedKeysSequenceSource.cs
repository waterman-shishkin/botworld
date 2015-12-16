using System;
using System.Collections.Generic;

namespace botworld.bl.tests
{
	public class PredefinedKeysSequenceSource : IKeysSequenceSource
	{
		private readonly Queue<ConsoleKey> keysSequence;

		public PredefinedKeysSequenceSource(ConsoleKey[] keysSequence)
		{
			this.keysSequence = new Queue<ConsoleKey>(keysSequence);
		}

		public ConsoleKey GetNextKey()
		{
			return keysSequence.Dequeue();
		}
	}
}