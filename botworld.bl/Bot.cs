namespace botworld.bl
{
	public class Bot : IEntity
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
			HP -= damage;
			if (HP < 0)
				HP = 0;
			return IsDead;
		}

		public InvasionResponseAction ChooseInvasionResponseAction(IEntity guest)
		{
			return botIntelligence.ChooseInvasionResponseAction(PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public BotInfo PrepareBotInfo()
		{
			return new BotInfo(PrepareEntityInfo(), Direction);
		}

		public AttackResponseAction ChooseAttackResponseAction(IEntity guest)
		{
			return botIntelligence.ChooseAttackResponseAction(PrepareBotInfo(), guest.PrepareEntityInfo());
		}

		public EntityInfo PrepareEntityInfo()
		{
			return new EntityInfo(Type, HP, AttackStrength, DefenceStrength, Location, CanShareCell, IsCollectable);
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
			Direction = direction;
		}

		public BotAction ChooseNextAction()
		{
			return botIntelligence.ChooseNextAction(PrepareBotInfo());
		}
	}
}
