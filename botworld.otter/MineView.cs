using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class MineView : Entity
	{
		private readonly Mine mine;
		private bool stateChanged;

		public MineView(Mine mine, Image image, int cellSize) :
			base(mine.Location.X * cellSize, mine.Location.Y * cellSize, image)
		{
			this.mine = mine;
			mine.OnStateChange += OnStateChange;
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
			if (mine.IsDead)
				RemoveSelf();
		}
	}
}