using System;

namespace Charso.Core
{
    [Serializable]
    public sealed class CityProjectDefinition
    {
        public string Id;
        public string DisplayName;
        public int CoinCost;
    }

    /// <summary>
    /// Central balancing values. Ad and purchase grants still require trusted provider verification;
    /// these constants are not proof that an ad was watched or a purchase was paid.
    /// </summary>
    public static class RewardEconomy
    {
        public const int RewardedAdCoins = 20;
        public const int MaxRewardedAdsPerServerDay = 3;

        public static readonly CityProjectDefinition[] CityProjects =
        {
            new CityProjectDefinition { Id = "fountain", DisplayName = "آب‌نمای میدان", CoinCost = 130 },
            new CityProjectDefinition { Id = "bazaar", DisplayName = "سقف و دکان‌های بازار", CoinCost = 180 },
            new CityProjectDefinition { Id = "garden", DisplayName = "باغ و آبراهه‌ها", CoinCost = 160 },
            new CityProjectDefinition { Id = "homes", DisplayName = "نمای خانه‌های محله", CoinCost = 220 },
            new CityProjectDefinition { Id = "workshop", DisplayName = "کارگاه مرمت", CoinCost = 180 },
            new CityProjectDefinition { Id = "gate", DisplayName = "دروازه شهر", CoinCost = 250 },
            new CityProjectDefinition { Id = "courtyard", DisplayName = "حیاط مجموعه", CoinCost = 300 },
            new CityProjectDefinition { Id = "landmark", DisplayName = "بنای شاخص محله", CoinCost = 350 }
        };

        public static int PuzzleReward(int difficulty)
        {
            if (difficulty <= 1) return 8;
            if (difficulty == 2) return 10;
            return 12;
        }

        public static bool TryGetCityProject(string id, out CityProjectDefinition project)
        {
            for (int i = 0; i < CityProjects.Length; i++)
            {
                if (string.Equals(CityProjects[i].Id, id, StringComparison.Ordinal))
                {
                    project = CityProjects[i];
                    return true;
                }
            }
            project = null;
            return false;
        }
    }
}