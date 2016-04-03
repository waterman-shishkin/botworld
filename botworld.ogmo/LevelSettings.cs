using System.Collections.Generic;
using System.Drawing;
using botworld.bl;

namespace botworld.ogmo
{
	public class LevelSettings
	{
		public string Title { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int CellSize { get; private set; }
		public Color BackgroundColor { get; private set; }
		public IReadOnlyDictionary<string, string> EntitiesImageFilenames { get; private set; }
		public IGameScenario Scenario { get; private set; }
		public IEnumerable<IEntity> Entities { get; private set; }

		public LevelSettings(string title, int width, int height, int cellSize, Color backgroundColor, IReadOnlyDictionary<string, string> entitiesImageFilenames, IGameScenario scenario, IEnumerable<IEntity> entities)
		{
			Title = title;
			Width = width;
			Height = height;
			CellSize = cellSize;
			BackgroundColor = backgroundColor;
			EntitiesImageFilenames = entitiesImageFilenames;
			Scenario = scenario;
			Entities = entities;
		}
	}
}
