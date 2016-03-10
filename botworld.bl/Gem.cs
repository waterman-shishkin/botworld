namespace botworld.bl
{
	public class Gem : IEntity, IPointsProvider
	{
		public Gem(int wp, Location location)
		{
			HP = 1;
			WP = wp;
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

		public double HP { get; private set; }

		public double AttackStrength { get { return 0; } }

		public double AutoDamageStrength { get { return 0; } }

		public double DefenceStrength { get { return 0; } }

		public int WP { get; private set; }

		public bool ImpactDamage(double damage)
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