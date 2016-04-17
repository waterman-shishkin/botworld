namespace botworld.bl
{
	public class Gem : BaseEntity, ICollectable, IPointsProvider
	{
		public Gem(int wp, Location location)
			: base(EntityType.Gem, location, true, 1, 0, 0, 0)
		{
			WP = wp;
		}

		public int WP { get; private set; }

		protected override void ImpactDamageInternal(double damage)
		{
			if (damage > 0)
				HP = 0;
		}

		public override InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return InvasionResponseAction.None;
		}

		public override AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return AttackResponseAction.None;
		}

		public override EntityInfo PrepareEntityInfo()
		{
			return new EntityInfo(Type, HP, AttackStrength, AutoDamageStrength, DefenseStrength, Location, CanShareCell, true, IsCollected, WP);
		}

		public bool IsCollected { get; private set; }

		public void OnCollected()
		{
			if (!IsCollected)
				ProceedStateChange(() => IsCollected = true);
		}
	}
}