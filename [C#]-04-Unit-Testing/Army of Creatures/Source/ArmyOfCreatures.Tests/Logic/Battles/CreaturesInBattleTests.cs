﻿namespace ArmyOfCreatures.Tests.Logic.Battles
{
    using System;
    using System.Linq;

    using ArmyOfCreatures.Logic.Battles;
    using ArmyOfCreatures.Tests.Fakes;

    using Moq;
    using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
    using NUnit.Framework;

    [TestFixture]
    public class CreaturesInBattleTests
    {
        [Test]
        public void Constructor_ShouldAssignCorrectValues()
        {
            var creature = new FakeCreature();
            var count = 10;

            var actualCreaturesInBattle = new CreaturesInBattle(creature, count);

            Assert.That(actualCreaturesInBattle,
                Is.InstanceOf<CreaturesInBattle>()
                .With.Property("Creature").SameAs(creature)
                .And.Property("PermanentAttack").EqualTo(creature.Attack)
                .And.Property("PermanentDefense").EqualTo(creature.Defense)
                .And.Property("TotalHitPoints").EqualTo(creature.HealthPoints * count));
        }

        [Test]
        public void TotalHitPoints_GetShouldReturnZero_WhenTotalHitPointsFieldValueIsLessthanZero()
        {
            var creature = new FakeCreature();
            var count = 10;

            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattleAsPrivateObject.SetField("totalHitPoints", int.MinValue);
            var actualTotalHitPoints = creaturesInBattle.TotalHitPoints;

            Assert.That(actualTotalHitPoints, Is.EqualTo(0));
        }

        [TestCase(4)]
        [TestCase(6)]
        [TestCase(5)]
        [TestCase(1892)]
        public void Count_GetShouldReturnCorrectValue(int totalHitPoints)
        {
            var creature = new FakeCreature();
            var count = 10;

            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattleAsPrivateObject.SetField("totalHitPoints", totalHitPoints);
            var expectedCount = (int)Math.Ceiling((double)totalHitPoints / creature.HealthPoints);
            var actualCount = creaturesInBattle.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Count_GetShouldReturnZero_WhenActualValueIsLessthanZero(int totalHitPoints)
        {
            var creature = new FakeCreature();
            var count = 10;

            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattleAsPrivateObject.SetField("totalHitPoints", totalHitPoints);
            var actualCount = creaturesInBattle.Count;

            Assert.That(actualCount, Is.EqualTo(0));
        }

        [Test]
        public void DealDamage_ShouldThrowArgumentNullException_WhenDefenderParametereIsNull()
        {
            ICreaturesInBattle defender = null;

            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            Assert.That(() => creaturesInBattle.DealDamage(defender),
                Throws.ArgumentNullException.With.Message.Contains("defender"));
        }

        [Test]
        public void DealDamage_ShouldApplyAttackBonusCorrectly_WhenCreaturesInBattleCurrentAttackIsLargerThanDefenderParameterCurrentDefense()
        {
            var mockDefender = new Mock<ICreaturesInBattle>();
            mockDefender.SetupGet(mock => mock.CurrentDefense).Returns(0);

            var creature = new FakeCreature();
            var count = 20;
            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattle.DealDamage(mockDefender.Object);
            var actualLastDamage = creaturesInBattleAsPrivateObject.GetField("lastDamage");
            var expectedBonusDamage = (decimal)(creature.Attack * 0.05) + 1;
            var expectedLastDamage = (decimal)(count * creature.Damage) * expectedBonusDamage;

            Assert.That(actualLastDamage, Is.EqualTo(expectedLastDamage));
        }

        [Test]
        public void DealDamage_ShouldNotApplyDamageBonusOrReduction_WhenCreaturesInBattleCurrentAttackIsEqualToThanDefenderParameterCurrentDefense()
        {
            var mockDefender = new Mock<ICreaturesInBattle>();
            mockDefender.SetupGet(mock => mock.CurrentDefense).Returns(FakeCreature.FakeAttack);

            var creature = new FakeCreature();
            var count = 20;
            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattle.DealDamage(mockDefender.Object);
            var actualLastDamage = creaturesInBattleAsPrivateObject.GetField("lastDamage");
            var expectedLastDamage = (decimal)(count * creature.Damage);

            Assert.That(actualLastDamage, Is.EqualTo(expectedLastDamage));
        }

        [Test]
        public void DealDamage_ShouldApplyAttackReductionCorrectly_WhenCreaturesInBattleCurrentAttackIsLessThanDefenderParameterCurrentDefense()
        {
            var mockDefender = new Mock<ICreaturesInBattle>();
            mockDefender.SetupGet(mock => mock.CurrentDefense).Returns(10);

            var creature = new FakeCreature();
            var count = 20;
            var creaturesInBattle = new CreaturesInBattle(creature, count);
            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);

            creaturesInBattle.DealDamage(mockDefender.Object);
            var actualLastDamage = creaturesInBattleAsPrivateObject.GetField("lastDamage");
            var expectedBonusReduction = 1m - (decimal)(creature.Attack * 0.025);
            var expectedLastDamage = (decimal)(count * creature.Damage) * expectedBonusReduction;

            Assert.That(actualLastDamage, Is.EqualTo(expectedLastDamage));
        }

        [Test]
        public void DealDamage_ShouldSetDefenderParameterTotalHitPointsWithCorrectValue()
        {
            var mockDefender = new Mock<ICreaturesInBattle>();
            mockDefender.SetupGet(mock => mock.CurrentDefense).Returns(FakeCreature.FakeAttack);

            var creature = new FakeCreature();
            var count = 20;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            creaturesInBattle.DealDamage(mockDefender.Object);
            var expectedLastDamage = (decimal)(count * creature.Damage);

            mockDefender.VerifySet(
                mock => mock.TotalHitPoints = It.Is<int>(i => i == -((int)expectedLastDamage)),
                Times.Once());
        }

        [Test]
        public void DealDamage_ShouldAccessCreatureSpeciltiesChangeDamageWhenAttacking()
        {
            var mockDefender = new Mock<ICreaturesInBattle>();
            mockDefender.SetupGet(mock => mock.CurrentDefense).Returns(FakeCreature.FakeAttack);

            var creature = new FakeCreature();
            var count = 20;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            creaturesInBattle.DealDamage(mockDefender.Object);
            var actualSpecialty = (FakeSpecialty)creature.Specialties.First();

            Assert.That(actualSpecialty.MethodsAccessedWhenDealDamageAreCalled, Is.True);
        }

        [Test]
        public void Skip_ShouldIncrementPermanentDefenseWithThree()
        {
            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            var initialPermanentDefense = creaturesInBattle.PermanentDefense;
            creaturesInBattle.Skip();
            var afterSkipPermanentDefense = creaturesInBattle.PermanentDefense;

            Assert.That(afterSkipPermanentDefense - initialPermanentDefense, Is.EqualTo(3));
        }

        [Test]
        public void Skip_ShouldAccessCreatureSpecialtyApplyOnSkip()
        {
            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            creaturesInBattle.Skip();
            var actualSpecialty = (FakeSpecialty)creature.Specialties.First();

            Assert.That(actualSpecialty.MethodsAccessedWhenSkipAreCalled, Is.True);
        }

        [Test]
        public void StartNewTurn_ShouldSetCorrectValueToCurrentAttack_WhenCalled()
        {
            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            var expectedValue = int.MaxValue;
            creaturesInBattle.PermanentAttack = expectedValue;
            creaturesInBattle.StartNewTurn();

            Assert.That(creaturesInBattle.CurrentAttack, Is.EqualTo(expectedValue));
        }

        [Test]
        public void StartNewTurn_ShouldSetCorrectValueToCurrentDefense_WhenCalled()
        {
            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            var expectedValue = int.MaxValue;
            creaturesInBattle.PermanentDefense = expectedValue;
            creaturesInBattle.StartNewTurn();

            Assert.That(creaturesInBattle.CurrentDefense, Is.EqualTo(expectedValue));
        }

        [Test]
        public void StartNewTurn_ShouldSetCorrectValueToLastDamage_WhenCalled()
        {
            var creature = new FakeCreature();
            var count = 10;
            var creaturesInBattle = new CreaturesInBattle(creature, count);

            creaturesInBattle.StartNewTurn();

            var creaturesInBattleAsPrivateObject = new MSTest.PrivateObject(creaturesInBattle);
            var actualLastDamage = creaturesInBattleAsPrivateObject.GetField("lastDamage");

            Assert.That(actualLastDamage, Is.EqualTo((decimal)0));
        }
    }
}
