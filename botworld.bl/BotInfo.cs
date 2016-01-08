namespace botworld.bl
{
	public class BotInfo
	{
		public BotInfo(double hp, Location location, Direction direction)
		{
			HP = hp;
			Location = location;
			Direction = direction;
		}

		public double HP { get; private set; }

		public Location Location { get; private set; }

		public Direction Direction { get; private set; }
	}
}