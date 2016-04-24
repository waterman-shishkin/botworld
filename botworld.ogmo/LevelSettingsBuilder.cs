using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using botworld.bl;
using Newtonsoft.Json.Linq;

namespace botworld.ogmo
{
	public class LevelSettingsBuilder
	{
		private XDocument project;
		private XDocument level;
		private IKeysSequenceSource keysSequenceSource;

		public LevelSettingsBuilder ForProject(XDocument projectDocument)
		{
			project = projectDocument;
			return this;
		}

		public LevelSettingsBuilder ForLevel(XDocument levelDocument)
		{
			level = levelDocument;
			return this;
		}

		public LevelSettingsBuilder ForPlayerInput(IKeysSequenceSource keysSequenceSource)
		{
			this.keysSequenceSource = keysSequenceSource;
			return this;
		}

		public LevelSettings Build()
		{
			var colorElement = project.XPathSelectElement(@"project/BackgroundColor");
			var backgroundColor = Color.FromArgb(ParsingHelper.ParseIntAttribute(colorElement, "A"), ParsingHelper.ParseIntAttribute(colorElement, "R"), ParsingHelper.ParseIntAttribute(colorElement, "G"), ParsingHelper.ParseIntAttribute(colorElement, "B"));

			var gridElement = project.XPathSelectElement(@"project/LayerDefinitions/LayerDefinition[Name = 'Entities']/Grid");
			var cellWidth = ParsingHelper.ParseIntElement(gridElement, "Width");
			var cellHeight = ParsingHelper.ParseIntElement(gridElement, "Height");
			if (cellWidth != cellHeight)
				throw new ArgumentException("Ширина и высота ячейки должны быть равны");

			var entitiesElements = project.XPathSelectElements(@"project/EntityDefinitions/EntityDefinition");
			var imageFilenames = entitiesElements.ToDictionary(e => e.Attribute("Name").Value,
				e => e.Element("ImageDefinition").Attribute("ImagePath").Value);

			var levelElement = level.XPathSelectElement(@"level");
			var title = levelElement.Attribute("Title").Value;
			var widthPixels = ParsingHelper.ParseIntAttribute(levelElement, "width");
			var width = widthPixels / cellWidth;
			if (widthPixels != width * cellWidth)
				throw new ArgumentException("Ширина уровня должна быть кратна размеру ячейки");
			var heightPixels = ParsingHelper.ParseIntAttribute(levelElement, "height");
			var height = heightPixels / cellHeight;
			if (heightPixels != height * cellHeight)
				throw new ArgumentException("Высота уровня должна быть кратна размеру ячейки");

			var scenarioJson = JObject.Parse(levelElement.Attribute("ScenarioJSON").Value);
			var scenario = new GameScenariosFactory().Create(scenarioJson);

			entitiesElements = level.XPathSelectElements(@"level/Entities/*");
			var entitiesFactory = new EntitiesFactory(cellWidth, keysSequenceSource);
			var entities = entitiesElements.Select(entitiesFactory.Create);

			return new LevelSettings(title, width, height, cellWidth, backgroundColor, imageFilenames, scenario, entities);
		}
	}
}
