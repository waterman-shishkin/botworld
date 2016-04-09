using System.Collections.Generic;
using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class MapView
	{
		private readonly IMap map;
		private readonly EntitiesViewFactory factory;

		public MapView(IMap map, int cellSize, IReadOnlyDictionary<string, string> entitiesImageFilenames, string levelsDirPath)
		{
			this.map = map;
			factory = new EntitiesViewFactory(cellSize, entitiesImageFilenames, levelsDirPath);
			InitScene();
		}

		private void InitScene()
		{
			Scene = new Scene();
			foreach (var entity in map.GetEntities())
				Scene.Add(factory.Create(entity));
		}

		public Scene Scene { get; private set; }
	}
}