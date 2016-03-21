namespace botworld.bl
{
	public class Mine : IEntity
	{
		public Mine(double attackStrength, Location location)
		{
			HP = 1;
			AttackStrength = attackStrength;
			Location = location;
		}

		public EntityType Type
		{
			get { return EntityType.Mine; }
		}

		public Location Location { get; private set; }

		public bool CanShareCell
		{
			get { return false; }
		}

		public bool IsCollectable
		{
			get { return false; }
		}

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public double HP { get; private set; }

		public double AttackStrength { get; private set; }

		public double AutoDamageStrength { get { return 1; } }

		public double DefenseStrength { get { return 0; } }

		public bool ImpactDamage(double damage)
		{
			if (damage > 0)
				HP = 0;
			return IsDead;
		}

		public InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return InvasionResponseAction.Attack;
		}

		public AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return AttackResponseAction.None;
		}
	}
}