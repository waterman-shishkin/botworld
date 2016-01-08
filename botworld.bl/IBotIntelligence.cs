namespace botworld.bl
{
	public interface IBotIntelligence
	{
		BotAction ChooseNextAction(BotInfo botInfo);
		InvasionResponseAction ChooseInvasionResponseAction(BotInfo botInfo, EntityInfo guestInfo);
		AttackResponseAction ChooseAttackResponseAction(BotInfo botInfo, EntityInfo guestInfo);
	}
}
