using System;
using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class OtterKeysSequenceSource : IKeysSequenceSource
	{
		public ConsoleKeyInfo GetNextKey()
		{
			var input = Input.Instance;
			if (input.KeyPressed(Key.Space))
				return new ConsoleKeyInfo(default(char), ConsoleKey.Spacebar, false, false, false);
			if (input.KeyPressed(Key.Return))
				return new ConsoleKeyInfo(default(char), ConsoleKey.Enter, false, false, false);
			var controlPressed = input.KeyDown(Key.LControl) || input.KeyDown(Key.RControl);
			if (input.KeyPressed(Key.Up))
				return new ConsoleKeyInfo(default(char), ConsoleKey.UpArrow, false, false, controlPressed);
			if (input.KeyPressed(Key.Right))
				return new ConsoleKeyInfo(default(char), ConsoleKey.RightArrow, false, false, controlPressed);
			if (input.KeyPressed(Key.Down))
				return new ConsoleKeyInfo(default(char), ConsoleKey.DownArrow, false, false, controlPressed);
			if (input.KeyPressed(Key.Left))
				return new ConsoleKeyInfo(default(char), ConsoleKey.LeftArrow, false, false, controlPressed);

			return new ConsoleKeyInfo();
		}
	}
}