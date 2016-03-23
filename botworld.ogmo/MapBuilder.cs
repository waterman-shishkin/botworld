using botworld.bl;

namespace botworld.ogmo
{
	public class MapBuilder
	{
		public IMap Build(LevelSettings settings)
		{
			var map = new Map(settings.Width, settings.Height);
			foreach (var entity in settings.Entities)
				map.Add(entity);
			return map;
		}
	}
}
