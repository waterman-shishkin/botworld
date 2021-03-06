﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace botworld.bl
{
	public class HumanControlBotIntelligence : IBotIntelligence
	{
		private static readonly Dictionary<ConsoleKey, Direction> keyToDirection = new Dictionary<ConsoleKey, Direction>
		{
			{ ConsoleKey.UpArrow, Direction.North },
			{ ConsoleKey.LeftArrow, Direction.West },
			{ ConsoleKey.RightArrow, Direction.East },
			{ ConsoleKey.DownArrow, Direction.South }
		};
		private readonly IKeysSequenceSource keysSource;
		private readonly Queue<BotAction> actionsQueue = new Queue<BotAction>();

		public HumanControlBotIntelligence(IKeysSequenceSource keysSource)
		{
			this.keysSource = keysSource;
		}

		public BotAction ChooseNextAction(BotInfo botInfo, Dictionary<Location, IEnumerable<EntityInfo>> neighborsInfo)
		{
			if (actionsQueue.Any())
				return actionsQueue.Dequeue();

			var keyPressed = keysSource.GetNextKey();
			switch (keyPressed.Key)
			{
				case ConsoleKey.Spacebar:
					return BotAction.Act;
				case ConsoleKey.Enter:
					return BotAction.Collect;
			}

			if (!IsDirectionKey(keyPressed))
				return BotAction.None;

			EnqueueTurnCommands(botInfo.Direction, keyPressed);

			actionsQueue.Enqueue((keyPressed.Modifiers & ConsoleModifiers.Control) != 0 ? BotAction.Explore : BotAction.Step);

			return actionsQueue.Dequeue();
		}

		public InvasionResponseAction ChooseInvasionResponseAction(BotInfo botInfo, EntityInfo guestInfo)
		{
			return InvasionResponseAction.None;
		}

		public AttackResponseAction ChooseAttackResponseAction(BotInfo botInfo, EntityInfo guestInfo)
		{
			return AttackResponseAction.None;
		}

		public void OnDamage(double previousHP, double newHP)
		{
		}

		public void OnRotation(Direction previousDirection, Direction newDirection)
		{
		}

		public void OnMove(Location previousLocation, Location newLocation)
		{
		}

		public void OnWPChange(int previousWP, int newWP)
		{
		}

		public void OnCollect(EntityInfo entityInfo)
		{
		}

		public void OnExplore(IEnumerable<EntityInfo> info)
		{
		}

		private void EnqueueTurnCommands(Direction direction, ConsoleKeyInfo keyPressed)
		{
			var desiredDirection = keyToDirection[keyPressed.Key];
			var turnsNeeded = desiredDirection - direction;
			if (turnsNeeded > 2)
				turnsNeeded -= 4;
			else if (turnsNeeded <= -2)
				turnsNeeded += 4;
			for (var i = 0; i < Math.Abs(turnsNeeded); i++)
				actionsQueue.Enqueue(turnsNeeded > 0 ? BotAction.TurnRight : BotAction.TurnLeft);
		}

		private static bool IsDirectionKey(ConsoleKeyInfo keyPressed)
		{
			return keyToDirection.ContainsKey(keyPressed.Key);
		}
	}
}