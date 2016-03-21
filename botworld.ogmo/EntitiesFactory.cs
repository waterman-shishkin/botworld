using System;
using System.Xml.Linq;
using botworld.bl;

namespace botworld.ogmo
{
	public class EntitiesFactory
	{
		private readonly int cellSize;

		public EntitiesFactory(int cellSize)
		{
			this.cellSize = cellSize;
		}

		public IEntity Create(XElement element)
		{
			var entityType = element.Name.LocalName;
			var location = new Location(ParsingHelper.ParseIntAttribute(element, "x") / cellSize, ParsingHelper.ParseIntAttribute(element, "y") / cellSize);
			switch (entityType)
			{
				case "wall":
					return new Wall(ParsingHelper.ParseDoubleAttribute(element, "HP"), ParsingHelper.ParseDoubleAttribute(element, "AttackStrength"), ParsingHelper.ParseDoubleAttribute(element, "DefenseStrength"), location);
				case "gem":
					return new Gem(ParsingHelper.ParseIntAttribute(element, "WP"), location);
				case "mine":
					return new Mine(ParsingHelper.ParseDoubleAttribute(element, "AttackStrength"), location);
				default:
					throw new ArgumentException(string.Format("Неизвестный тип сущности: '{0}'", entityType));
			}
		}
	}
}