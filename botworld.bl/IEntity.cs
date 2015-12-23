namespace botworld.bl
{
	public interface IEntity
	{
		EntityType Type { get; }
		Location Location { get; } 
		bool CanShareCell { get; }
		bool IsCollectable { get; }
		bool IsDead { get; }
		float HP { get; }
		float AttackStrength { get; }
		float DefenceStrength { get; }
		bool ImpactDamage(float damage);
		InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);
		AttackResponseAction ChooseAttackResponseAction(IEntity guest);
	}
}
