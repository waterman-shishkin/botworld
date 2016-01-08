namespace botworld.bl
{
	public class Bot
	{
		private readonly IBotIntelligence botIntelligence;
		public string Name { get; private set; }

		public Bot(string name, IBotIntelligence botIntelligence)
		{
			Name = name;
			this.botIntelligence = botIntelligence;
		}

		public BotAction ChooseNextAction()
		{
			//todo: нужно создавать объект BotInfo
			return botIntelligence.ChooseNextAction(null);
		}
	}
}
