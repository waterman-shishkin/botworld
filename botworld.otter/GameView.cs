using System.Collections.Generic;
using botworld.bl;
using Otter;
using Color = System.Drawing.Color;
using Game = Otter.Game;

namespace botworld.otter
{
	public class GameView
	{
		private readonly IGame game;
		private readonly Game otterGame;
		private readonly MapView mapView;

		public GameView(string title, IGame game, int cellSize, Color backgroundColor, IReadOnlyDictionary<string, string> entitiesImageFilenames, string levelsDirPath)
		{
			this.game = game;
			otterGame = new Game(title, game.Map.Width*cellSize, game.Map.Height*cellSize, 30);
			otterGame.Color = new Otter.Color((float)backgroundColor.R / 255, (float)backgroundColor.G / 255, (float)backgroundColor.B / 255, (float)backgroundColor.A / 255);
			mapView = new MapView(game.Map, cellSize, entitiesImageFilenames, levelsDirPath);
		}

		public void Start()
		{
			otterGame.OnUpdate += OnUpdate;
			otterGame.Start(mapView.Scene);
		}

		private void OnUpdate()
		{
			if (game.GameOver || !game.Tick()) 
				return;

			ShowGameOver();
		}

		private void ShowGameOver()
		{
			var entity = new Entity(otterGame.HalfWidth, otterGame.HalfHeight);
			var text = new Text("Game over!", 60);
			text.TextStyle = TextStyle.Bold;
			text.ShadowColor = Otter.Color.Red;
			text.ShadowX = 1;
			text.ShadowY = 3;
			text.CenterOrigin();
			entity.AddGraphic(text);
			mapView.Scene.Add(entity);
		}
	}
}
