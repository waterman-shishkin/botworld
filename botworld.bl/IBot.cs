namespace botworld.bl
{
	public interface IBot : IEntity
	{
		BotInfo PrepareBotInfo();
		Direction Direction { get; }
		void UpdateDirection(Direction direction);
		BotAction ChooseNextAction();
	}
}