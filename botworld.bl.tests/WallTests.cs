using NUnit.Framework;

namespace botworld.bl.tests
{
	[TestFixture]
	public class WallTests
	{
		[Test]
		public void Type_Returns_Wall()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.Type, Is.EqualTo(EntityType.Wall));
		}

		[Test]
		public void Location_Returns_LocationSetByConstructor()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.Location.X, Is.EqualTo(2));
			Assert.That(wall.Location.Y, Is.EqualTo(4));
		}

		[Test]
		public void HP_Returns_HPSetByConstructor()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.HP, Is.EqualTo(100));
		}

		[Test]
		public void CanShareCell_Returns_False()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.CanShareCell, Is.False);
		}

		[Test]
		public void IsCollectable_Returns_False()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.IsCollectable, Is.False);
		}

		[Test]
		public void AttackStrength_Returns_AttackStrengthSetByConstructor()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.AttackStrength, Is.EqualTo(10));
		}

		[Test]
		public void AutoDamageStrength_Returns_Zero()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.AutoDamageStrength, Is.EqualTo(0));
		}

		[Test]
		public void DefenceStrength_Returns_DefenceStrengthSetByConstructor()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			Assert.That(wall.DefenseStrength, Is.EqualTo(10));
		}

		[Test]
		public void PrepareEntityInfo_Returns_ProperInfo()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			var entityInfo = wall.PrepareEntityInfo();

			Assert.That(entityInfo.Type, Is.EqualTo(EntityType.Wall));
			Assert.That(entityInfo.Location.X, Is.EqualTo(2));
			Assert.That(entityInfo.Location.Y, Is.EqualTo(4));
			Assert.That(entityInfo.CanShareCell, Is.False);
			Assert.That(entityInfo.IsCollectable, Is.False);
			Assert.That(entityInfo.HP, Is.EqualTo(100));
			Assert.That(entityInfo.AttackStrength, Is.EqualTo(10));
			Assert.That(entityInfo.AutoDamageStrength, Is.EqualTo(0));
			Assert.That(entityInfo.DefenseStrength, Is.EqualTo(10));
		}

		[Test]
		public void ImpactDamage_ForZeroDamage_ResultsNotIsDeadAndNotChangedHP()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			wall.ImpactDamage(0);

			Assert.That(wall.HP, Is.EqualTo(100));
			Assert.That(wall.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForWeakDamage_ResultsNotIsDeadAndHPDecrease()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			wall.ImpactDamage(25);

			Assert.That(wall.HP, Is.EqualTo(75));
			Assert.That(wall.IsDead, Is.False);
		}

		[Test]
		public void ImpactDamage_ForStrongDamage_ResultsIsDeadAndZeroHP()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			wall.ImpactDamage(250);

			Assert.That(wall.HP, Is.EqualTo(0));
			Assert.That(wall.IsDead, Is.True);
		}

		[Test]
		public void ChooseInvasionResponseAction_Always_ReturnnsAttack()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			var action = wall.ChooseInvasionResponseAction(null);

			Assert.That(action, Is.EqualTo(InvasionResponseAction.Attack));
		}

		[Test]
		public void ChooseAttackResponseAction_Always_ReturnnsNone()
		{
			var wall = new Wall(100, 10, 10, new Location(2, 4));

			var action = wall.ChooseAttackResponseAction(null);

			Assert.That(action, Is.EqualTo(AttackResponseAction.None));
		}
	}
}
