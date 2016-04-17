using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class GemTests
	{
		[Test]
		public void Type_Returns_Gem()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.Type, Is.EqualTo(EntityType.Gem));
		}

		[Test]
		public void WP_Returns_PointsSetByConstructor()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.WP, Is.EqualTo(100));
		}

		[Test]
		public void Location_Returns_LocationSetByConstructor()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.Location.X, Is.EqualTo(2));
			Assert.That(gem.Location.Y, Is.EqualTo(4));
		}

		[Test]
		public void CanShareCell_Returns_True()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.CanShareCell, Is.True);
		}

		[Test]
		public void AttackStrength_Returns_Zero()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.AttackStrength, Is.EqualTo(0));
		}

		[Test]
		public void AutoDamageStrength_Returns_Zero()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.AutoDamageStrength, Is.EqualTo(0));
		}

		[Test]
		public void DefenceStrength_Returns_Zero()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.DefenseStrength, Is.EqualTo(0));
		}

		[Test]
		public void PrepareEntityInfo_Returns_ProperInfo()
		{
			var gem = new Gem(100, new Location(2, 4));

			var entityInfo = gem.PrepareEntityInfo();

			Assert.That(entityInfo.Type, Is.EqualTo(EntityType.Gem));
			Assert.That(entityInfo.Location.X, Is.EqualTo(2));
			Assert.That(entityInfo.Location.Y, Is.EqualTo(4));
			Assert.That(entityInfo.CanShareCell, Is.True);
			Assert.That(entityInfo.IsCollectable, Is.True);
			Assert.That(entityInfo.HP, Is.EqualTo(1));
			Assert.That(entityInfo.AttackStrength, Is.EqualTo(0));
			Assert.That(entityInfo.AutoDamageStrength, Is.EqualTo(0));
			Assert.That(entityInfo.DefenseStrength, Is.EqualTo(0));
			Assert.That(entityInfo.WP, Is.EqualTo(100));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_ResultsNotIsDead()
		{
			var gem = new Gem(100, new Location(2, 4));

			gem.ImpactDamage(0);

			Assert.That(gem.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForNotZeroDamage_ResultsIsDead()
		{
			var gem = new Gem(100, new Location(2, 4));

			gem.ImpactDamage(0.0001);

			Assert.That(gem.IsDead, Is.True);
		}

		[Test]
		public void ChooseInvasionResponseAction_Always_ReturnnsNone()
		{
			var gem = new Gem(100, new Location(2, 4));

			var action = gem.ChooseInvasionResponseAction(null);

			Assert.That(action, Is.EqualTo(InvasionResponseAction.None));
		}

		[Test]
		public void ChooseAttackResponseAction_Always_ReturnnsNone()
		{
			var gem = new Gem(100, new Location(2, 4));

			var action = gem.ChooseAttackResponseAction(null);

			Assert.That(action, Is.EqualTo(AttackResponseAction.None));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_DoNotTriggerEvent()
		{
			var eventCounter = 0;
			var gem = new Gem(100, new Location(2, 4));
			gem.OnStateChange += (sender, args) => eventCounter++;

			gem.ImpactDamage(0);

			Assert.That(eventCounter, Is.EqualTo(0));
		}

		[Test]
		public void ImpactDamage_ForNonZeroDamage_TriggersEvent()
		{
			var eventCounter = 0;
			EntityEventArgs eventArgs = null;
			object eventSender = null;
			var gem = new Gem(100, new Location(2, 4));
			gem.OnStateChange += (sender, args) =>
			{
				eventCounter++;
				eventSender = sender;
				eventArgs = args;
			};

			gem.ImpactDamage(25);

			Assert.That(eventCounter, Is.EqualTo(1));
			Assert.That(eventSender, Is.EqualTo(gem));
			Assert.That(eventArgs.PreviousStateInfo.HP, Is.EqualTo(1));
			Assert.That(eventArgs.CurrentStateInfo.HP, Is.EqualTo(0));
		}

		[Test]
		public void IsCollected_ForJustCreatedGem_ReturnsFalse()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.IsCollected, Is.False);
		}

		[Test]
		public void IsCollected_ForCollectedGem_ReturnsTrue()
		{
			var gem = new Gem(100, new Location(2, 4));

			gem.OnCollected();

			Assert.That(gem.IsCollected, Is.True);
		}

		[Test]
		public void OnCollected_CalledOnce_TriggersEvent()
		{
			var eventCounter = 0;
			EntityEventArgs eventArgs = null;
			object eventSender = null;
			var gem = new Gem(100, new Location(2, 4));
			gem.OnStateChange += (sender, args) =>
			{
				eventCounter++;
				eventSender = sender;
				eventArgs = args;
			};

			gem.OnCollected();

			Assert.That(eventCounter, Is.EqualTo(1));
			Assert.That(eventSender, Is.EqualTo(gem));
			Assert.That(eventArgs.PreviousStateInfo.IsCollected, Is.False);
			Assert.That(eventArgs.CurrentStateInfo.IsCollected, Is.True);
		}

		[Test]
		public void OnCollected_CalledTwice_TriggersEventOnce()
		{
			var eventCounter = 0;
			var gem = new Gem(100, new Location(2, 4));
			gem.OnStateChange += (sender, args) => eventCounter++;

			gem.OnCollected();
			gem.OnCollected();

			Assert.That(eventCounter, Is.EqualTo(1));
		}
	}
}
