namespace botworld.bl
{
	public class Wall : IEntity
	{
		public Wall(float hp, float attackStrength, float defenceStrength, Location location)
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

		public float HP { get; private set; }

		public float AttackStrength { get; private set; }

		public float DefenceStrength { get; private set; }

		public bool ImpactDamage(float damage)
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