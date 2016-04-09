using System.Collections.Generic;
using botworld.bl;
using Color = System.Drawing.Color;
using Game = Otter.Game;

namespace botworld.otter
{
	public class GameView
	{
		private readonly IGame game;
		private readonly Game otterGame;
		private MapView mapView;

		public GameView(string title, IGame game, int cellSize, Color backgroundColor, IReadOnlyDictionary<string, string> entitiesImageFilenames, string levelsDirPath)
		{
			this.game = game;
			otterGame = new Game(title, game.Map.Width*cellSize, game.Map.Height*cellSize, 30);
			otterGame.Color = new Otter.Color((float)backgroundColor.R / 255, (float)backgroundColor.G / 255, (float)backgroundColor.B / 255, (float)backgroundColor.A / 255);
			mapView = new MapView(game.Map, cellSize, entitiesImageFilenames, levelsDirPath);
		}

		public void Start()
		{
			otterGame.Start(mapView.Scene);
		}
	}
}
