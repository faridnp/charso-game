using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class RewardLedgerTests
    {
        [Test]
        public void RewardCanOnlyBeAppliedOnce()
        {
            var state = new RewardLedgerState { SoftCurrency = 10 };
            Assert.IsTrue(RewardLedger.TryApply(state, "level-1-complete", 8));
            Assert.IsFalse(RewardLedger.TryApply(state, "level-1-complete", 8));
            Assert.AreEqual(18, state.SoftCurrency);
        }

        [Test]
        public void CannotSpendBelowZero()
        {
            var state = new RewardLedgerState { SoftCurrency = 3 };
            Assert.IsFalse(RewardLedger.TryApply(state, "hint-1", -4));
            Assert.AreEqual(3, state.SoftCurrency);
        }

        [Test]
        public void PuzzleRewardScalesByEditorialDifficulty()
        {
            Assert.AreEqual(8, RewardEconomy.PuzzleReward(1));
            Assert.AreEqual(10, RewardEconomy.PuzzleReward(2));
            Assert.AreEqual(12, RewardEconomy.PuzzleReward(3));
            Assert.AreEqual(12, RewardEconomy.PuzzleReward(99));
        }

        [Test]
        public void CityProjectCatalogHasUniqueIdsAndPositiveCosts()
        {
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (CityProjectDefinition project in RewardEconomy.CityProjects)
            {
                Assert.IsTrue(ids.Add(project.Id), "Duplicate project: " + project.Id);
                Assert.Greater(project.CoinCost, 0);
            }
            Assert.AreEqual(8, RewardEconomy.CityProjects.Length);
        }

        [Test]
        public void CannotExceedMaximumBalance()
        {
            var state = new RewardLedgerState { SoftCurrency = 999999 };
            Assert.IsFalse(RewardLedger.TryApply(state, "reward-overflow", 1));
            Assert.AreEqual(999999, state.SoftCurrency);
        }
    }
}