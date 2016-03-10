namespace botworld.bl
{
	public interface IEntity
	{
		EntityType Type { get; }
		Location Location { get; } 
		bool CanShareCell { get; }
		bool IsCollectable { get; }
		bool IsDead { get; }
		double HP { get; }
		double AttackStrength { get; }
		double AutoDamageStrength { get; }
		double DefenceStrength { get; }
		bool ImpactDamage(double damage);
		InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);
		AttackResponseAction ChooseAttackResponseAction(IEntity guest);
	}
}
