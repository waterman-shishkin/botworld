using botworld.bl;

namespace botworld.ogmo
{
	public class GameBuilder
	{
		private readonly MapBuilder mapBuilder = new MapBuilder();

		public IGame Build(LevelSettings settings)
		{
			var map = mapBuilder.Build(settings);
			var game = new Game(map, settings.Scenario);
			return game;
		}
	}
}
