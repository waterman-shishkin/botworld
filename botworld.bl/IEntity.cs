namespace botworld.bl
{
	public interface IEntity
	{
		EntityType Type { get; }
		Location Location { get; } 
		bool CanShareCell { get; }
		bool IsDead { get; }
		double HP { get; }
		double AttackStrength { get; }
		double AutoDamageStrength { get; }
		double DefenseStrength { get; }
		bool ImpactDamage(double damage);
		InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);
		AttackResponseAction ChooseAttackResponseAction(IEntity guest);
		EntityInfo PrepareEntityInfo();
		event EntityStateChangeHandler OnStateChange;
	}

	public delegate void EntityStateChangeHandler(object sender, EntityEventArgs args);
}
