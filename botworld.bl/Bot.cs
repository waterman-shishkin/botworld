using System;
using System.Collections.Generic;

namespace botworld.bl
{
	public class Bot : IBot
	{
		private readonly IBotIntelligence botIntelligence;
		private readonly List<IEntity> collectedEntities = new List<IEntity>();

		public Bot(string name, double hp, double attackStrength, double autoDamageStrength, double defenseStrength, Location location, Direction direction, IBotIntelligence botIntelligence)
		{
			Name = name;
			HP = hp;
			AttackStrength = attackStrength;
			AutoDamageStrength = autoDamageStrength;
			DefenseStrength = defenseStrength;
			Location = location;
			Direction = direction;
			WP = 0;
			this.botIntelligence = botIntelligence;
		}
		
		public string Name { get; private set; }

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public double HP { get; private set; }

		public int WP { get; private set; }

		public double AttackStrength { get; private set; }

		public double AutoDamageStrength { get; private set; }

		public double DefenseStrength { get; private set; }

		public bool ImpactDamage(double damage)
		{
			var previousHP = HP;
			HP -= damage;
			if (HP < 0)
				HP = 0;
			if (HP != previousHP)
				botIntelligence.OnDamage(previousHP, HP);
			return IsDead;
		}

		public InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return botIntelligence.ChooseInvasionResponseAction(this.PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return botIntelligence.ChooseAttackResponseAction(this.PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public EntityType Type
		{
			get { return EntityType.Bot; }
		}

		public Location Location { get; private set; }

		public bool CanShareCell
		{
			get { return true; }
		}

		public bool IsCollectable
		{
			get { return false; }
		}

		public Direction Direction { get; private set; }

		public IEnumerable<IEntity> CollectedEntities
		{
			get { return collectedEntities.AsReadOnly(); }
		}

		public void UpdateDirection(Direction direction)
		{
			var previousDirection = Direction;
			Direction = direction;
			if (Direction != previousDirection)
				botIntelligence.OnRotation(previousDirection, Direction);
		}

		public int UpdateWP(int wpDiff)
		{
			var previousWP = WP;
			WP += wpDiff;
			if (WP != previousWP)
				botIntelligence.OnWPChange(previousWP, WP);
			return WP;
		}

		public void UpdateLocation(Location location)
		{
			var previousLocation = Location;
			Location = location;
			if (Location != previousLocation)
				botIntelligence.OnMove(previousLocation, Location);
		}

		public BotAction ChooseNextAction(Dictionary<Location, IEnumerable<EntityInfo>> neighborsInfo)
		{
			return botIntelligence.ChooseNextAction(this.PrepareBotInfo(), neighborsInfo);
		}

		public void Collect(IEntity entity)
		{
			if (!entity.IsCollectable)
				throw new InvalidOperationException(string.Format("The entity {0} is not collectable", entity));

			collectedEntities.Add(entity);
			botIntelligence.OnCollect(entity.PrepareEntityInfo());
		}

		public void OnExplore(IEnumerable<EntityInfo> info)
		{
			botIntelligence.OnExplore(info);
		}
	}
}
