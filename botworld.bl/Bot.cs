using System;
using System.Collections.Generic;

namespace botworld.bl
{
	public class Bot : BaseEntity, IBot
	{
		private readonly List<IEntity> collectedEntities = new List<IEntity>();

		public Bot(string name, double hp, double attackStrength, double autoDamageStrength, double defenseStrength, Location location, Direction direction, IBotIntelligence intelligence)
			: base(EntityType.Bot, location, true, hp, attackStrength, autoDamageStrength, defenseStrength)
		{
			Name = name;
			Direction = direction;
			WP = 0;
			Intelligence = intelligence;
		}
		
		public string Name { get; private set; }

		public IBotIntelligence Intelligence { get; private set; }

		public int WP { get; private set; }

		protected override void ImpactDamageInternal(double damage)
		{
			if (damage <= 0)
				return;

			var previousHP = HP;
			HP -= damage;
			if (HP < 0)
				HP = 0;
			if (HP != previousHP)
				Intelligence.OnDamage(previousHP, HP);
		}

		public override InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return Intelligence.ChooseInvasionResponseAction(PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public override AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return Intelligence.ChooseAttackResponseAction(PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public Direction Direction { get; private set; }

		public IEnumerable<IEntity> CollectedEntities
		{
			get { return collectedEntities.AsReadOnly(); }
		}

		public void UpdateDirection(Direction direction)
		{
			if (Direction != direction)
				ProceedStateChange(() => UpdateDirectionInternal(direction));
		}

		private void UpdateDirectionInternal(Direction direction)
		{
			var previousDirection = Direction;
			Direction = direction;
			Intelligence.OnRotation(previousDirection, Direction);
		}

		public int UpdateWP(int wpDiff)
		{
			if (wpDiff > 0)
				ProceedStateChange(() => UpdateWPInternal(wpDiff));
			return WP;
		}

		private void UpdateWPInternal(int wpDiff)
		{
			var previousWP = WP;
			WP += wpDiff;
			Intelligence.OnWPChange(previousWP, WP);
		}

		public void UpdateLocation(Location location)
		{
			if (Location != location)
				ProceedStateChange(() => UpdateLocationInternal(location));
		}

		private void UpdateLocationInternal(Location location)
		{
			var previousLocation = Location;
			Location = location;
			Intelligence.OnMove(previousLocation, Location);
		}

		public BotAction ChooseNextAction(Dictionary<Location, IEnumerable<EntityInfo>> neighborsInfo)
		{
			return Intelligence.ChooseNextAction(PrepareBotInfo(), neighborsInfo);
		}

		public void Collect(IEntity entity)
		{
			if (!(entity is ICollectable))
				throw new InvalidOperationException(string.Format("The entity {0} is not collectable", entity));

			collectedEntities.Add(entity);
			Intelligence.OnCollect(entity.PrepareEntityInfo());
		}

		public override EntityInfo PrepareEntityInfo()
		{
			return PrepareBotInfo();
		}

		public BotInfo PrepareBotInfo()
		{
			return new BotInfo(HP, AttackStrength, AutoDamageStrength, DefenseStrength, Location, WP, Direction);
		}

		public void OnExplore(IEnumerable<EntityInfo> info)
		{
			Intelligence.OnExplore(info);
		}
	}
}