namespace botworld.bl
{
	public static class EntitiesExtensions
	{
		public static EntityInfo PrepareEntityInfo(this IEntity entity)
		{
			return new EntityInfo(entity.Type, entity.HP, entity.AttackStrength, entity.DefenceStrength, entity.Location, entity.CanShareCell, entity.IsCollectable);
		}

		public static BotInfo PrepareBotInfo(this IBot bot)
		{
			return new BotInfo(bot.PrepareEntityInfo(), bot.Direction);
		}
	}
}
