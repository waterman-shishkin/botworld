namespace botworld.bl
{
	public static class EntitiesExtensions
	{
		public static EntityInfo PrepareEntityInfo(this IEntity entity)
		{
			return entity is IBot ? PrepareBotInfo((IBot)entity) : PrepareEntityInfoInternal(entity);
		}

		public static BotInfo PrepareBotInfo(this IBot bot)
		{
			return new BotInfo(PrepareEntityInfoInternal(bot), bot.Direction);
		}

		private static EntityInfo PrepareEntityInfoInternal(IEntity entity)
		{
			return new EntityInfo(entity.Type, entity.HP, entity.AttackStrength, entity.DefenceStrength, entity.Location, entity.CanShareCell, entity.IsCollectable);
		}
	}
}
