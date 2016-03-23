using System;
using System.Xml.Linq;
using botworld.bl;

namespace botworld.ogmo
{
	public static class ParsingHelper
	{
		public static int ParseIntAttribute(XElement element, string attributeName)
		{
			return int.Parse(element.Attribute(attributeName).Value);
		}

		public static int ParseIntElement(XElement element, string valueElementName)
		{
			return int.Parse(element.Element(valueElementName).Value);
		}

		public static double ParseDoubleAttribute(XElement element, string attributeName)
		{
			return double.Parse(element.Attribute(attributeName).Value);
		}

		public static Direction ParseDirectionAttribute(XElement element, string attributeName)
		{
			return (Direction)Enum.Parse(typeof(Direction), element.Attribute(attributeName).Value, true);
		}

		public static Location ParseLocation(XElement element, int cellSize)
		{
			return new Location(ParseIntAttribute(element, "x") / cellSize, ParseIntAttribute(element, "y") / cellSize);
		}
	}
}