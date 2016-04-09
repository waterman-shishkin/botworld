using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class GemView : Entity
	{
		private readonly Gem gem;

		public GemView(Gem gem, Image image, int cellSize):
			base(gem.Location.X * cellSize, gem.Location.Y * cellSize, image)
		{
			this.gem = gem;
		}
	}
}