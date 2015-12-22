namespace botworld.bl
{
	class Wall : IEntity
	{
		public Wall(decimal hp, decimal attackStrength, decimal defenceStrength)
		{
			HP = hp;
			AttackStrength = attackStrength;
			DefenceStrength = defenceStrength;
		}

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

		public decimal HP { get; private set; }

		public decimal AttackStrength { get; private set; }

		public decimal DefenceStrength { get; private set; }

		public bool ImpactDamage(decimal damage)
		{
			HP -= damage;
			if (HP < 0)
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