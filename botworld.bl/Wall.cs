namespace botworld.bl
{
	public class Wall : BaseEntity
	{
		public Wall(double hp, double attackStrength, double defenseStrength, Location location)
			: base(EntityType.Wall, location, false, hp, attackStrength, 0, defenseStrength)
		{
		}

		public override InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return InvasionResponseAction.Attack;
		}

		public override AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return AttackResponseAction.None;
		}
	}
}