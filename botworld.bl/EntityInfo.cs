namespace botworld.bl
{
	public class EntityInfo
	{
		public EntityInfo(EntityType type, double hp, double attackStrength, double autoDamageStrength, double defenseStrength, Location location, bool canShareCell, bool isCollectable, bool isCollected, int wp)
		{
			Type = type;
			HP = hp;
			AttackStrength = attackStrength;
			AutoDamageStrength = autoDamageStrength;
			DefenseStrength = defenseStrength;
			Location = location;
			CanShareCell = canShareCell;
			IsCollectable = isCollectable;
			IsCollected = isCollected;
			WP = wp;
		}

		public EntityType Type { get; private set; }
		public Location Location { get; private set; }
		public bool CanShareCell { get; private set; }
		public bool IsCollectable { get; private set; }
		public bool IsCollected { get; private set; }
		public double HP { get; private set; }
		public double AttackStrength { get; private set; }
		public double AutoDamageStrength { get; private set; }
		public double DefenseStrength { get; private set; }
		public int WP { get; private set; }

		public bool IsDead
		{
			get { return HP <= 0; }
		}
	}
}