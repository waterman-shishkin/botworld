using System.Collections.Generic;

namespace botworld.bl
{
	public interface IBotIntelligence
	{
		BotAction ChooseNextAction(BotInfo botInfo, Dictionary<Location, IEnumerable<EntityInfo>> neighborsInfo);
		InvasionResponseAction ChooseInvasionResponseAction(BotInfo botInfo, EntityInfo guestInfo);
		AttackResponseAction ChooseAttackResponseAction(BotInfo botInfo, EntityInfo guestInfo);
		void OnDamage(double previousHP, double newHP);
		void OnRotation(Direction previousDirection, Direction newDirection);
		void OnMove(Location previousLocation, Location newLocation);
		void OnWPChange(int previousWP, int newWP);
		void OnCollect(EntityInfo entityInfo);
		void OnExplore(IEnumerable<EntityInfo> info);
	}
}
