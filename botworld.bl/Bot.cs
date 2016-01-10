namespace botworld.bl
{
	public class Bot : IBot
	{
		private readonly IBotIntelligence botIntelligence;

		public Bot(string name, double hp, double attackStrength, double defenceStrength, Location location, Direction direction, IBotIntelligence botIntelligence)
		{
			Name = name;
			HP = hp;
			AttackStrength = attackStrength;
			DefenceStrength = defenceStrength;
			Location = location;
			Direction = direction;
			this.botIntelligence = botIntelligence;
		}
		
		public string Name { get; private set; }

		public bool IsDead
		{
			get { return HP <= 0; }
		}

		public double HP { get; private set; }

		public double AttackStrength { get; private set; }

		public double DefenceStrength { get; private set; }

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

		public void UpdateDirection(Direction direction)
		{
			var previousDirection = Direction;
			Direction = direction;
			if (Direction != previousDirection)
				botIntelligence.OnRotation(previousDirection, Direction);
		}

		public void UpdateLocation(Location location)
		{
			var previousLocation = Location;
			Location = location;
			if (Location != previousLocation)
				botIntelligence.OnMove(previousLocation, Location);
		}

		public BotAction ChooseNextAction()
		{
			return botIntelligence.ChooseNextAction(this.PrepareBotInfo());
		}
	}
}
