namespace botworld.bl
{
	public interface IEntity
	{
		bool CanShareCell { get; }
		bool IsCollectable { get; }
		decimal Health { get; }
		decimal AttackStrength { get; }
		decimal DefenceStrength { get; }
		InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);
	}
}
