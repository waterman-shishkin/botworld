using botworld.bl;
using Otter;

namespace botworld.otter
{
	public class BotView : Entity
	{
		private readonly IBot bot;
		private bool stateChanged;
		private readonly int cellSize;

		public BotView(IBot bot, Image image, int cellSize):
			base(bot.Location.X * cellSize, bot.Location.Y * cellSize, image)
		{
			this.bot = bot;
			this.cellSize = cellSize;
			bot.OnStateChange += OnStateChange;
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
			if (bot.IsDead)
			{
				RemoveSelf();
				return;
			}
			X = bot.Location.X * cellSize;
			Y = bot.Location.Y * cellSize;
		}
	}
}