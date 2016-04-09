using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class MineView : Entity
	{
		private readonly Mine mine;

		public MineView(Mine mine, Image image, int cellSize) :
			base(mine.Location.X * cellSize, mine.Location.Y * cellSize, image)
		{
			this.mine = mine;
		}
	}
}