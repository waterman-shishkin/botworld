namespace botworld.bl
{
	public class Mine : BaseEntity
	{
		public Mine(double attackStrength, Location location)
			: base(EntityType.Mine, location, false, 1, attackStrength, 1, 0)
		{
		}

		protected override void ImpactDamageInternal(double damage)
		{
			if (damage > 0)
				HP = 0;
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