using System;

namespace botworld.bl
{
	public abstract class BaseEntity : IEntity
	{
		protected BaseEntity(EntityType type, Location location, bool canShareCell, double hp, double attackStrength, double autoDamageStrength, double defenseStrength)
		{
			Type = type;
			Location = location;
			CanShareCell = canShareCell;
			HP = hp;
			AttackStrength = attackStrength;
			AutoDamageStrength = autoDamageStrength;
			DefenseStrength = defenseStrength;
		}

		public EntityType Type { get; private set; }

		public Location Location { get; protected set; }

		public bool CanShareCell { get; private set; }

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public double HP { get; protected set; }

		public double AttackStrength { get; protected set; }

		public double AutoDamageStrength { get; protected set; }

		public double DefenseStrength { get; protected set; }

		public bool ImpactDamage(double damage)
		{

			if (damage > 0 && !IsDead)
				ProceedStateChange(() => ImpactDamageInternal(damage));
			return IsDead;
		}

		protected virtual void ImpactDamageInternal(double damage)
		{
			HP -= damage;
			if (HP < 0)
				HP = 0;
		}

		public abstract InvasionResponseAction ChooseInvasionResponseAction(IEntity guest);

		public abstract AttackResponseAction ChooseAttackResponseAction(IEntity guest);

		public virtual EntityInfo PrepareEntityInfo()
		{
			return new EntityInfo(Type, HP, AttackStrength, AutoDamageStrength, DefenseStrength, Location, CanShareCell, false, false, 0);
		}

		protected void ProceedStateChange(Action action)
		{
			var previousState = PrepareEntityInfo();
			action.Invoke();
			var newState = PrepareEntityInfo();
			var handler = OnStateChange;
			if (handler != null)
				handler(this, new EntityEventArgs(previousState, newState));
		}

		public event EntityStateChangeHandler OnStateChange;
	}
}