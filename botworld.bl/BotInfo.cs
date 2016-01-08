namespace botworld.bl
{
	public class BotInfo
	{
		public BotInfo(float hp, Location location)
		{
			HP = hp;
			Location = location;
		}

		public float HP { get; private set; }

		public Location Location { get; private set; }
	}
}