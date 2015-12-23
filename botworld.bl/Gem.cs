namespace botworld.bl
{
	public class Gem : IEntity, IPointsProvider
	{
		public Gem(int points, Location location)
		{
			HP = 1;
			Points = points;
			Location = location;
		}

		public EntityType Type
		{
			get { return EntityType.Gem; }
		}

		public Location Location { get; private set; }

		public bool CanShareCell
		{
			get { return true; }
		}

		public bool IsCollectable
		{
			get { return true; }
		}

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public float HP { get; private set; }

		public float AttackStrength { get { return 0; } }

		public float DefenceStrength { get { return 0; } }

		public int Points { get; private set; }

		public bool ImpactDamage(float damage)
		{
			if (damage > 0)
				HP = 0;
			return IsDead;
		}

		public InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return InvasionResponseAction.None;
		}

		public AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return AttackResponseAction.None;
		}
	}
}