using System;

namespace Charso.Core
{
    [Serializable]
    public sealed class SaveGameData
    {
        public int SchemaVersion = 3;
        public int LevelIndex;
        public int SoftCurrency = 20;
        public int RestorationStage;
        public int SolvedLevels;
        public bool CurrentLevelSolved;
        public bool CurrentLevelRestored;
        public string LastCompletedPuzzleId = string.Empty;
        public string[] AppliedTransactionIds = Array.Empty<string>();
        public string[] OwnedCityProjectIds = Array.Empty<string>();
    }

    [Serializable]
    internal sealed class SaveEnvelope
    {
        public int FormatVersion = 1;
        public string Payload;
        public string Checksum;
    }

    [Serializable]
    internal sealed class LegacySaveV2
    {
        public int SchemaVersion = 2;
        public int LevelIndex;
        public int SoftCurrency = 20;
        public int RestorationStage;
        public int SolvedLevels;
        public bool CurrentLevelSolved;
        public bool CurrentLevelRestored;
        public string LastCompletedPuzzleId = string.Empty;
        public string[] AppliedTransactionIds = Array.Empty<string>();
    }

    [Serializable]
    internal sealed class LegacySaveV1
    {
        public int SchemaVersion = 1;
        public int LevelIndex;
        public int Coins = 20;
        public int RestorationStage;
        public int SolvedLevels;
        public bool CurrentLevelSolved;
        public bool CurrentLevelRestored;
    }
}