using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class WallView : Entity
	{
		private readonly Wall wall;

		public WallView(Wall wall, Image image, int cellSize):
			base(wall.Location.X * cellSize, wall.Location.Y * cellSize, image)
		{
			this.wall = wall;
		}
	}
}