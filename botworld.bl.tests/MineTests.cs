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
		public void CanShareCell_Returns_True()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.CanShareCell, Is.True);
		}

		[Test]
		public void IsCollectable_Returns_False()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.IsCollectable, Is.False);
		}

		[Test]
		public void AttackStrength_Returns_AttackStrengthSetByConstructor()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.AttackStrength, Is.EqualTo(100));
		}

		[Test]
		public void DefenceStrength_Returns_Zero()
		{
			var mine = new Mine(100, new Location(2, 4));

			Assert.That(mine.DefenceStrength, Is.EqualTo(0));
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
	}
}
