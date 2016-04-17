using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class MineTests
	{
		[Test]
		public void Type_Returns_Mine()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.Type, Is.EqualTo(EntityType.Mine));
		}

		[Test]
		public void Location_Returns_LocationSetByConstructor()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.Location.X, Is.EqualTo(2));
			Assert.That(mine.Location.Y, Is.EqualTo(4));
		}

		[Test]
		public void CanShareCell_Returns_False()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.CanShareCell, Is.False);
		}

		[Test]
		public void AttackStrength_Returns_AttackStrengthSetByConstructor()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.AttackStrength, Is.EqualTo(100));
		}

		[Test]
		public void AutoDamageStrength_Returns_Unity()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.AutoDamageStrength, Is.EqualTo(1));
		}

		[Test]
		public void DefenceStrength_Returns_Zero()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.DefenseStrength, Is.EqualTo(0));
		}

		[Test]
		public void PrepareEntityInfo_Returns_ProperInfo()
		{
			var mine = new Mine(100, new Location(2, 4));

			var entityInfo = mine.PrepareEntityInfo();

			Assert.That(entityInfo.Type, Is.EqualTo(EntityType.Mine));
			Assert.That(entityInfo.Location.X, Is.EqualTo(2));
			Assert.That(entityInfo.Location.Y, Is.EqualTo(4));
			Assert.That(entityInfo.CanShareCell, Is.False);
			Assert.That(entityInfo.IsCollectable, Is.False);
			Assert.That(entityInfo.HP, Is.EqualTo(1));
			Assert.That(entityInfo.AttackStrength, Is.EqualTo(100));
			Assert.That(entityInfo.AutoDamageStrength, Is.EqualTo(1));
			Assert.That(entityInfo.DefenseStrength, Is.EqualTo(0));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_ResultsNotIsDead()
		{
			var mine = new Mine(100, new Location(2, 4));

			mine.ImpactDamage(0);

			Assert.That(mine.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForNotZeroDamage_ResultsIsDead()
		{
			var mine = new Mine(100, new Location(2, 4));

			mine.ImpactDamage(0.0001);

			Assert.That(mine.IsDead, Is.True);
		}

		[Test]
		public void ChooseInvasionResponseAction_Always_ReturnnsAttack()
		{
			var mine = new Mine(100, new Location(2, 4));

			var action = mine.ChooseInvasionResponseAction(null);

			Assert.That(action, Is.EqualTo(InvasionResponseAction.Attack));
		}

		[Test]
		public void ChooseAttackResponseAction_Always_ReturnnsNone()
		{
			var mine = new Mine(100, new Location(2, 4));

			var action = mine.ChooseAttackResponseAction(null);

			Assert.That(action, Is.EqualTo(AttackResponseAction.None));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_DoNotTriggerEvent()
		{
			var eventCounter = 0;
			var mine = new Mine(100, new Location(2, 4));
			mine.OnStateChange += (sender, args) => eventCounter++;

			mine.ImpactDamage(0);

			Assert.That(eventCounter, Is.EqualTo(0));
		}

		[Test]
		public void ImpactDamage_ForNonZeroDamage_TriggersEvent()
		{
			var eventCounter = 0;
			EntityEventArgs eventArgs = null;
			object eventSender = null;
			var mine = new Mine(100, new Location(2, 4));
			mine.OnStateChange += (sender, args) =>
			{
				eventCounter++;
				eventSender = sender;
				eventArgs = args;
			};

			mine.ImpactDamage(25);

			Assert.That(eventCounter, Is.EqualTo(1));
			Assert.That(eventSender, Is.EqualTo(mine));
			Assert.That(eventArgs.PreviousStateInfo.HP, Is.EqualTo(1));
			Assert.That(eventArgs.CurrentStateInfo.HP, Is.EqualTo(0));
		}
	}
}
