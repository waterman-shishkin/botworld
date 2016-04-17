using System;
using System.Collections.Generic;
using System.Linq;

namespace botworld.bl
{
	public class Game : IGame
	{
		private readonly IGameScenario scenario;

		public IMap Map { get; private set; }

		public bool GameOver
		{
			get { return scenario.IsGameOver(Map); }
		}

		public IEnumerable<IBot> Winners
		{
			get { return scenario.GetWinners(Map); }
		}

		public Game(IMap map, IGameScenario scenario)
		{
			Map = map;
			this.scenario = scenario;
		}

		public bool Tick()
		{
			if (GameOver)
				return true;

			foreach (var bot in Map.GetBots().ToArray())
			{
				if (bot.IsDead)
					continue;

				var action = bot.ChooseNextAction(Map.GetNeighborsInfo(bot));
				switch (action)
				{
					case BotAction.None:
						break;
					case BotAction.TurnLeft:
					case BotAction.TurnRight:
						ProceedTurnAction(bot, action);
						break;
					case BotAction.Step:
						ProceedStepAction(bot);
						break;
					case BotAction.Explore:
						ProceedExploreAction(bot);
						break;
					case BotAction.Collect:
						ProceedCollectAction(bot);
						break;
					case BotAction.Act:
						ProceedActAction(bot);
						break;
					default:
						throw new InvalidOperationException(string.Format("Unknown bot action {0}", action));
				}
			}
			return GameOver;
		}

		private void ProceedActAction(IBot bot)
		{
			var locationToAffect = bot.Location.GetNeighborLocationInDirection(bot.Direction);
			if (!Map.IsInRange(locationToAffect))
				return;
			var entitiesToAffect = Map.GetEntities(locationToAffect).ToArray();
			foreach (var entity in entitiesToAffect)
			{
				ProceedAttack(bot, entity);
				if (bot.IsDead)
					break;
			}
		}

		private void ProceedAttack(IEntity aggressor, IEntity defender)
		{
			ProceedImpact(aggressor, defender);
			if (defender.IsDead)
				return;
			var action = defender.ChooseAttackResponseAction(aggressor);
			if (action == AttackResponseAction.Attack)
				ProceedImpact(defender, aggressor);
		}

		private void ProceedImpact(IEntity aggressor, IEntity defender)
		{
			var damage = aggressor.AttackStrength - defender.DefenseStrength;
			if (damage > 0)
				defender.ImpactDamage(damage);
			if (defender.IsDead)
				Map.Remove(defender);
			var autoDamage = aggressor.AutoDamageStrength;
			if (autoDamage > 0)
				aggressor.ImpactDamage(autoDamage);
			if (aggressor.IsDead)
				Map.Remove(aggressor);
		}

		private void ProceedStepAction(IBot bot)
		{
			var targetLocation = bot.Location.GetNeighborLocationInDirection(bot.Direction);
			if (!Map.IsInRange(targetLocation))
				return;
			var targetEntities = Map.GetEntities(targetLocation).ToArray();
			foreach (var entity in targetEntities)
			{
				var action = entity.ChooseInvasionResponseAction(bot);
				if (action != InvasionResponseAction.Attack)
					continue;
				ProceedAttack(entity, bot);
				if (bot.IsDead)
					break;
			}
			if (!bot.IsDead && Map.CanMoveBot(bot, targetLocation))
				Map.MoveBot(bot, targetLocation, bot.Direction);
		}

		private void ProceedCollectAction(IBot bot)
		{
			var collectableEntities = Map.GetEntities(bot.Location).Where(e => e != bot && e is ICollectable).ToArray();
			foreach (var entity in collectableEntities)
			{
				((ICollectable)entity).OnCollected();
				Map.Remove(entity);
				bot.Collect(entity);
				var pointsProvider = entity as IPointsProvider;
				if (pointsProvider != null)
					bot.UpdateWP(pointsProvider.WP);
			}
		}

		private void ProceedExploreAction(IBot bot)
		{
			var locationToExplore = bot.Location.GetNeighborLocationInDirection(bot.Direction);
			if (!Map.IsInRange(locationToExplore))
				return;
			Map.ExploreNeighborCell(bot);
		}

		private void ProceedTurnAction(IBot bot, BotAction action)
		{
			var direction = (int)bot.Direction;
			switch (action)
			{
				case BotAction.TurnLeft:
					direction--;
					if (direction < 0)
						direction += 4;
					break;
				case BotAction.TurnRight:
					direction++;
					if (direction > 3)
						direction -= 4;
					break;
			}
			Map.MoveBot(bot, bot.Location, (Direction)direction);
		}
	}
}