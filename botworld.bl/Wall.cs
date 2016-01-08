namespace botworld.bl
{
	public class Wall : IEntity
	{
		public Wall(double hp, double attackStrength, double defenceStrength, Location location)
		{
			HP = hp;
			AttackStrength = attackStrength;
			DefenceStrength = defenceStrength;
			Location = location;
		}

		public EntityType Type
		{
			get { return EntityType.Wall; }
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

		public double DefenceStrength { get; private set; }

		public bool ImpactDamage(double damage)
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

		public EntityInfo PrepareEntityInfo()
		{
			return new EntityInfo(Type, HP, AttackStrength, DefenceStrength, Location, CanShareCell, IsCollectable);
		}
	}
}