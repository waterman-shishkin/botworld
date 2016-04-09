using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class BotView : Entity
	{
		private readonly IBot bot;

		public BotView(IBot bot, Image image, int cellSize):
			base(bot.Location.X * cellSize, bot.Location.Y * cellSize, image)
		{
			this.bot = bot;
		}
	}
}