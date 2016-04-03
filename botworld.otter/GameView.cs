using System.Collections.Generic;
using System.Drawing;
using botworld.bl;
using Game = Otter.Game;

namespace botworld.otter
{
	public class GameView
	{
		private readonly Game otterGame;

		public GameView(string title, int width, int height, int cellSize, Color backgroundColor, IReadOnlyDictionary<string, string> entitiesImageFilenames, IGame game)
		{
			otterGame = new Game(title, width*cellSize, height*cellSize, 30);
			otterGame.Color = new Otter.Color((float)backgroundColor.R / 255, (float)backgroundColor.G / 255, (float)backgroundColor.B / 255, (float)backgroundColor.A / 255);
		}

		public void Start()
		{
			otterGame.Start();
		}
	}
}
