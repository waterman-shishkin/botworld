namespace botworld.bl
{
	public interface IBot : IEntity
	{
		Direction Direction { get; }
		int WP { get; }
		void UpdateDirection(Direction direction);
		void UpdateLocation(Location location);
		int UpdateWP(int wpDiff);
		BotAction ChooseNextAction();
	}
}