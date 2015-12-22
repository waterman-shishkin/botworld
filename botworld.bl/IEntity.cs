namespace botworld.bl
{
	public interface IEntity
	{
		bool CanShareCell { get; }
		bool IsCollectable { get; }
		bool IsDead { get; }
		decimal HP { get; }
		decimal AttackStrength { get; }
		decimal DefenceStrength { get; }
		bool ImpactDamage(decimal damage);
		InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);
		AttackResponseAction ChooseAttackResponseAction(IEntity guest);
	}
}
