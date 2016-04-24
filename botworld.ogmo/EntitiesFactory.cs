using System;
using System.Xml.Linq;
using botworld.bl;
using Newtonsoft.Json.Linq;

namespace botworld.ogmo
{
	public class EntitiesFactory
	{
		private readonly int cellSize;
		private readonly IKeysSequenceSource keysSequenceSource;

		public EntitiesFactory(int cellSize, IKeysSequenceSource keysSequenceSource)
		{
			this.cellSize = cellSize;
			this.keysSequenceSource = keysSequenceSource;
		}

		public IEntity Create(XElement element)
		{
			var entityType = element.Name.LocalName;
			var location = ParsingHelper.ParseLocation(element, cellSize);
			switch (entityType)
			{
				case "wall":
					return new Wall(ParsingHelper.ParseDoubleAttribute(element, "HP"), ParsingHelper.ParseDoubleAttribute(element, "AttackStrength"), ParsingHelper.ParseDoubleAttribute(element, "DefenseStrength"), location);
				case "gem":
					return new Gem(ParsingHelper.ParseIntAttribute(element, "WP"), location);
				case "mine":
					return new Mine(ParsingHelper.ParseDoubleAttribute(element, "AttackStrength"), location);
				case "bot":
					var intelligenceJson = JObject.Parse(element.Attribute("IntelligenceJSON").Value);
					var intelligence = new BotIntelligenceFactory(keysSequenceSource).Create(intelligenceJson);
					return new Bot(element.Attribute("Name").Value, ParsingHelper.ParseDoubleAttribute(element, "HP"), ParsingHelper.ParseDoubleAttribute(element, "AttackStrength"), ParsingHelper.ParseDoubleAttribute(element, "AutoDamageStrength"), ParsingHelper.ParseDoubleAttribute(element, "DefenseStrength"), location, ParsingHelper.ParseDirectionAttribute(element, "Direction"), intelligence);
				default:
					throw new ArgumentException(string.Format("Неизвестный тип сущности: '{0}'", entityType));
			}
		}
	}
}