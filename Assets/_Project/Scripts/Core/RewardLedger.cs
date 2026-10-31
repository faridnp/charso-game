using System;
using System.Collections.Generic;

namespace Charso.Core
{
    [Serializable]
    public sealed class RewardLedgerState
    {
        public int SoftCurrency;
        public string[] AppliedTransactionIds = Array.Empty<string>();
    }

    /// <summary>Local prototype ledger. Never use client-only balances for paid or competitive value.</summary>
    public static class RewardLedger
    {
        public static bool TryApply(RewardLedgerState state, string transactionId, int delta, int maximumBalance = 999999)
        {
            if (state == null || string.IsNullOrWhiteSpace(transactionId) || delta == 0 || maximumBalance < 0) return false;
            if (state.AppliedTransactionIds == null) state.AppliedTransactionIds = Array.Empty<string>();
            for (int i = 0; i < state.AppliedTransactionIds.Length; i++)
                if (string.Equals(state.AppliedTransactionIds[i], transactionId, StringComparison.Ordinal)) return false;

            long next = (long)state.SoftCurrency + delta;
            if (next < 0 || next > maximumBalance) return false;

            var ids = new List<string>(state.AppliedTransactionIds) { transactionId };
            state.AppliedTransactionIds = ids.ToArray();
            state.SoftCurrency = (int)next;
            return true;
        }
    }
}