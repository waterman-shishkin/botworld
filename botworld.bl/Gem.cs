namespace botworld.bl
{
	class Gem : IEntity, IPointsProvider
	{
		public Gem(int points)
		{
			HP = 1;
			Points = points;
		}

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

		public decimal HP { get; private set; }

		public decimal AttackStrength { get { return 0; } }

		public decimal DefenceStrength { get { return 0; } }

		public int Points { get; private set; }

		public bool ImpactDamage(decimal damage)
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