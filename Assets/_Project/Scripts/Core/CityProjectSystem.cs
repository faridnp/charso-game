using System;
using System.Collections.Generic;

namespace Charso.Core
{
    public static class CityProjectSystem
    {
        public static bool IsOwned(SaveGameData save, string projectId)
        {
            if (save == null || string.IsNullOrWhiteSpace(projectId) || save.OwnedCityProjectIds == null) return false;
            for (int i = 0; i < save.OwnedCityProjectIds.Length; i++)
                if (string.Equals(save.OwnedCityProjectIds[i], projectId, StringComparison.Ordinal)) return true;
            return false;
        }

        public static bool TryPurchase(SaveGameData save, string projectId, out string error)
        {
            error = string.Empty;
            if (save == null) { error = "ذخیره بازی در دسترس نیست."; return false; }
            if (!RewardEconomy.TryGetCityProject(projectId, out CityProjectDefinition project))
            { error = "این پروژه در فهرست شهر وجود ندارد."; return false; }
            if (IsOwned(save, projectId)) { error = "این بخش شهر قبلاً بازسازی شده است."; return false; }

            var ledger = new RewardLedgerState
            {
                SoftCurrency = save.SoftCurrency,
                AppliedTransactionIds = save.AppliedTransactionIds
            };
            if (!RewardLedger.TryApply(ledger, "city-project:" + projectId, -project.CoinCost))
            { error = "سکه کافی برای این پروژه نداری."; return false; }

            var owned = new List<string>(save.OwnedCityProjectIds ?? Array.Empty<string>());
            owned.Add(projectId);
            save.SoftCurrency = ledger.SoftCurrency;
            save.AppliedTransactionIds = ledger.AppliedTransactionIds;
            save.OwnedCityProjectIds = owned.ToArray();
            return true;
        }
    }
}