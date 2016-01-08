namespace botworld.bl
{
	public class Bot
	{
		private readonly IBotIntelligence botIntelligence;

		public Bot(string name, double hp, Location location, Direction direction, IBotIntelligence botIntelligence)
		{
			Name = name;
			HP = hp;
			Location = location;
			Direction = direction;
			this.botIntelligence = botIntelligence;
		}
		
		public string Name { get; private set; }

		public double HP { get; private set; }

		public Location Location { get; private set; }

		public Direction Direction { get; private set; }

		public void UpdateDirection(Direction direction)
		{
			Direction = direction;
		}

		public BotAction ChooseNextAction()
		{
			return botIntelligence.ChooseNextAction(new BotInfo(HP, Location, Direction));
		}
	}
}
