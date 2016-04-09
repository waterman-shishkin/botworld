using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class EntitiesViewFactory
	{
		private readonly int cellSize;
		private readonly IReadOnlyDictionary<string, string> entitiesImageFilenames;
		private readonly Dictionary<EntityType, Image> entitiesImages = new Dictionary<EntityType, Image>();

		public EntitiesViewFactory(int cellSize, IReadOnlyDictionary<string, string> entitiesImageFilenames, string levelsDirPath)
		{
			this.cellSize = cellSize;
			this.entitiesImageFilenames = entitiesImageFilenames.ToDictionary(kvp => kvp.Key, kvp => Path.Combine(levelsDirPath, kvp.Value), StringComparer.CurrentCultureIgnoreCase);
		}

		public Entity Create(IEntity entity)
		{
			var image = GetImage(entity.Type);
			switch (entity.Type)
			{
				case EntityType.Bot:
					return new BotView((IBot)entity, image, cellSize);
				case EntityType.Gem:
					return new GemView((Gem)entity, image, cellSize);
				case EntityType.Wall:
					return new WallView((Wall)entity, image, cellSize);
				case EntityType.Mine:
					return new MineView((Mine)entity, image, cellSize);
				default:
					throw new ArgumentException(string.Format("Неизвестный тип сущности: '{0}'", entity.Type));
			}
		}

		private Image GetImage(EntityType entityType)
		{
			if (!entitiesImages.ContainsKey(entityType))
				entitiesImages[entityType] = new Image(entitiesImageFilenames[entityType.ToString()]);
			return entitiesImages[entityType];
		}
	}
}