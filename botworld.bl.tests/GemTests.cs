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
		public void Points_Returns_PointsSetByConstructor()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.Points, Is.EqualTo(100));
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
		public void IsCollectable_Returns_True()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.IsCollectable, Is.True);
		}

		[Test]
		public void AttackStrength_Returns_Zero()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.AttackStrength, Is.EqualTo(0));
		}

		[Test]
		public void DefenceStrength_Returns_Zero()
		{
			var gem = new Gem(100, new Location(2, 4));

			Assert.That(gem.DefenceStrength, Is.EqualTo(0));
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
	}
}
