namespace botworld.bl
{
	class Bomb : IEntity
	{
		public Bomb(decimal attackStrength)
		{
			HP = 1;
			AttackStrength = attackStrength;
		}

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

		public decimal HP { get; private set; }

		public decimal AttackStrength { get; private set; }

		public decimal DefenceStrength { get { return 0; } }

		public bool ImpactDamage(decimal damage)
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