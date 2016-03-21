using System.Xml.Linq;

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
	}
}