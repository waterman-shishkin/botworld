namespace botworld.bl
{
	public interface IBot : IEntity
	{
		Direction Direction { get; }
		void UpdateDirection(Direction direction);
		void UpdateLocation(Location location);
		BotAction ChooseNextAction();
	}
}