using System;
using botworld.bl;
using Newtonsoft.Json.Linq;

namespace botworld.ogmo
{
	public class GameScenariosFactory
	{
		public IGameScenario Create(JObject scenarioSettings)
		{
			var scenarioType = (string)scenarioSettings["type"];
			switch (scenarioType)
			{
				case "points":
					return new PointsScenario((int)scenarioSettings["wp"]);
				default:
					throw new ArgumentException(string.Format("Неизвестный тип сценария: '{0}'", scenarioType));
			}
		}
	}
}