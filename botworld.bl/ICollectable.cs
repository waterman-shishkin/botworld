namespace botworld.bl
{
	public interface ICollectable
	{
		bool IsCollected { get; }
		void OnCollected();
	}
}