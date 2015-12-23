namespace botworld.bl
{
	public class Mine : IEntity
	{
		public Mine(float attackStrength, Location location)
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
			get { return true; }
		}

		public bool IsCollectable
		{
			get { return false; }
		}

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public float HP { get; private set; }

		public float AttackStrength { get; private set; }

		public float DefenceStrength { get { return 0; } }

		public bool ImpactDamage(float damage)
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