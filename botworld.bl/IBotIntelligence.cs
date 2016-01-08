namespace botworld.bl
{
	public interface IBotIntelligence
	{
		BotAction ChooseNextAction(BotInfo botInfo);
	}
}
