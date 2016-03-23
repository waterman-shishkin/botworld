using System.Xml.Linq;
using botworld.bl;

namespace botworld.ogmo
{
	public class GameLoader
	{
		private readonly LevelSettingsBuilder settingsBuilder = new LevelSettingsBuilder();
		private readonly GameBuilder gameBuilder = new GameBuilder();

		public LevelSettings LevelSettings { get; private set; }

		public IGame Load(string projectFilePath, string levelFilePath)
		{
			var projectDocument = XDocument.Load(projectFilePath);
			var levelDocument = XDocument.Load(levelFilePath);
			LevelSettings = settingsBuilder.ForProject(projectDocument).ForLevel(levelDocument).Build();
			var game = gameBuilder.Build(LevelSettings);
			return game;
		}
	}
}
