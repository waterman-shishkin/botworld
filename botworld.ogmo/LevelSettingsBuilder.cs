using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;

namespace botworld.ogmo
{
	public class LevelSettingsBuilder
	{
		private XDocument project;
		private XDocument level;

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

		public LevelSettings Build()
		{
			var colorElement = project.XPathSelectElement(@"project/BackgroundColor");
			var backgroundColor = Color.FromArgb(ParseIntAttribute(colorElement, "A"), ParseIntAttribute(colorElement, "R"),
				ParseIntAttribute(colorElement, "G"), ParseIntAttribute(colorElement, "B"));

			var gridElement = project.XPathSelectElement(@"project/LayerDefinitions/LayerDefinition[Name = 'Entities']/Grid");
			var cellWidth = ParseIntElement(gridElement, "Width");
			var cellHeight = ParseIntElement(gridElement, "Height");
			if (cellWidth != cellHeight)
				throw new ArgumentException("Ширина и высота ячейки должны быть равны");

			var entitiesElements = project.XPathSelectElements(@"project/EntityDefinitions/EntityDefinition");
			var imageFilenames = entitiesElements.ToDictionary(e => e.Attribute("Name").Value,
				e => e.Element("ImageDefinition").Attribute("ImagePath").Value);

			var levelElement = level.XPathSelectElement(@"level");
			var widthPixels = ParseIntAttribute(levelElement, "width");
			var width = widthPixels / cellWidth;
			if (widthPixels != width * cellWidth)
				throw new ArgumentException("Ширина уровня должна быть кратна размеру ячейки");
			var heightPixels = ParseIntAttribute(levelElement, "height");
			var height = heightPixels / cellHeight;
			if (heightPixels != height * cellHeight)
				throw new ArgumentException("Высота уровня должна быть кратна размеру ячейки");

			var scenarioJson = JObject.Parse(levelElement.Attribute("ScenarioJSON").Value);

			var scenario = new GameScenariosFactory().Create(scenarioJson);

			return new LevelSettings(width, height, cellWidth, backgroundColor, imageFilenames, scenario, null);
		}

		private static int ParseIntAttribute(XElement element, string attributeName)
		{
			return int.Parse(element.Attribute(attributeName).Value);
		}

		private static int ParseIntElement(XElement parentElement, string valueElementName)
		{
			return int.Parse(parentElement.Element(valueElementName).Value);
		}
	}
}
