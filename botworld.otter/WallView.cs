using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class WallView : Entity
	{
		private readonly Wall wall;
		private bool stateChanged;

		public WallView(Wall wall, Image image, int cellSize):
			base(wall.Location.X * cellSize, wall.Location.Y * cellSize, image)
		{
			this.wall = wall;
			wall.OnStateChange += OnStateChange;
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
			if (wall.IsDead)
				RemoveSelf();
		}
	}
}