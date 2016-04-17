namespace botworld.bl
{
	public class BotInfo : EntityInfo
	{
		public BotInfo(double hp, double attackStrength, double autoDamageStrength, double defenseStrength, Location location, int wp, Direction direction)
			: base(EntityType.Bot, hp, attackStrength, autoDamageStrength, defenseStrength, location, true, false, false, wp)
		{
			Direction = direction;
		}

		public BotInfo(EntityInfo entityInfo, Direction direction)
			: this(entityInfo.HP, entityInfo.AttackStrength, entityInfo.AutoDamageStrength, entityInfo.DefenseStrength, entityInfo.Location, entityInfo.WP, direction)
		{}

		public Direction Direction { get; private set; }
	}
}