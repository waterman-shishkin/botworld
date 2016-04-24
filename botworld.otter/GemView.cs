using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class GemView : Entity
	{
		private readonly Gem gem;
		private bool stateChanged;

		public GemView(Gem gem, Image image, int cellSize):
			base(gem.Location.X * cellSize, gem.Location.Y * cellSize, image)
		{
			this.gem = gem;
			gem.OnStateChange += OnStateChange;
		}

		private void OnStateChange(object sender, EntityEventArgs args)
		{
			stateChanged = true;
		}

		public override void Update()
		{
			base.Update();
			if (!stateChanged)
				return;

			stateChanged = false;
			if (gem.IsDead || gem.IsCollected)
				RemoveSelf();
		}
	}
}