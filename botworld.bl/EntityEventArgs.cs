namespace botworld.bl
{
	public class EntityEventArgs
	{
		public EntityEventArgs(EntityInfo previousStateInfo, EntityInfo currentStateInfo)
		{
			PreviousStateInfo = previousStateInfo;
			CurrentStateInfo = currentStateInfo;
		}

		public EntityInfo PreviousStateInfo { get; private set; }

		public EntityInfo CurrentStateInfo { get; private set; }
	}
}
