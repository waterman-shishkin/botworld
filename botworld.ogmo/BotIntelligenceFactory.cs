using System;
using botworld.bl;
using Newtonsoft.Json.Linq;

namespace botworld.ogmo
{
	public class BotIntelligenceFactory
	{
		private readonly IKeysSequenceSource keysSequenceSource;

		public BotIntelligenceFactory(IKeysSequenceSource keysSequenceSource)
		{
			this.keysSequenceSource = keysSequenceSource;
		}

		public IBotIntelligence Create(JObject intelligenceSettings)
		{
			var intelligenceType = (string)intelligenceSettings["type"];
			switch (intelligenceType)
			{
				case "human":
					return new HumanControlBotIntelligence(keysSequenceSource);
				default:
					throw new ArgumentException(string.Format("Неизвестный тип интеллекта: '{0}'", intelligenceType));
			}
		}
	}
}