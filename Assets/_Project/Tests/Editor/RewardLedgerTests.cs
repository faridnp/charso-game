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
        public void CannotExceedMaximumBalance()
        {
            var state = new RewardLedgerState { SoftCurrency = 999999 };
            Assert.IsFalse(RewardLedger.TryApply(state, "reward-overflow", 1));
            Assert.AreEqual(999999, state.SoftCurrency);
        }
    }
}