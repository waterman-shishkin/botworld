﻿namespace botworld.bl
{
	public class BotInfo : EntityInfo
	{
		public BotInfo(double hp, double attackStrength, double defenceStrength, Location location, int wp, Direction direction)
			: base(EntityType.Bot, hp, attackStrength, defenceStrength, location, true, false, wp)
		{
			Direction = direction;
		}

		public BotInfo(EntityInfo entityInfo, Direction direction)
			: this(entityInfo.HP, entityInfo.AttackStrength, entityInfo.DefenceStrength, entityInfo.Location, entityInfo.WP, direction)
		{}

		public Direction Direction { get; private set; }
	}
}