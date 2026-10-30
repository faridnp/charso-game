using System;
using Charso.Core;
using UnityEngine;

namespace Charso
{
    [Serializable]
    public sealed class PlayerProgress
    {
        public int SchemaVersion=3;
        public int LevelIndex;
        public int Coins=20;
        public int RestorationStage;
        public int SolvedLevels;
        public bool CurrentLevelSolved;
        public bool CurrentLevelRestored;
        public string LastCompletedPuzzleId=string.Empty;
        public string[] AppliedTransactionIds=Array.Empty<string>();
        public string[] OwnedCityProjectIds=Array.Empty<string>();
    }

    public static class SaveSystem
    {
        static SaveRepository Repository => new SaveRepository(Application.persistentDataPath,"charso-save.json");

        public static PlayerProgress Load()
        {
            SaveGameData data=Repository.LoadOrNew(out bool recovered,out bool migrated);
            if(recovered)Debug.LogWarning("Charso save recovered from the last known-good backup.");
            if(migrated)Debug.Log("Charso save migrated to schema v3.");
            return Clean(new PlayerProgress
            {
                SchemaVersion=3,
                LevelIndex=data.LevelIndex,
                Coins=data.SoftCurrency,
                RestorationStage=data.RestorationStage,
                SolvedLevels=data.SolvedLevels,
                CurrentLevelSolved=data.CurrentLevelSolved,
                CurrentLevelRestored=data.CurrentLevelRestored,
                LastCompletedPuzzleId=data.LastCompletedPuzzleId,
                AppliedTransactionIds=data.AppliedTransactionIds,
                OwnedCityProjectIds=data.OwnedCityProjectIds
            });
        }

        public static bool Store(PlayerProgress progress)
        {
            if(progress==null)return false;
            Clean(progress);
            try
            {
                Repository.Save(new SaveGameData
                {
                SchemaVersion=3,
                LevelIndex=progress.LevelIndex,
                SoftCurrency=progress.Coins,
                RestorationStage=progress.RestorationStage,
                SolvedLevels=progress.SolvedLevels,
                CurrentLevelSolved=progress.CurrentLevelSolved,
                CurrentLevelRestored=progress.CurrentLevelRestored,
                LastCompletedPuzzleId=progress.LastCompletedPuzzleId,
                    AppliedTransactionIds=progress.AppliedTransactionIds,
                    OwnedCityProjectIds=progress.OwnedCityProjectIds
                });
            }
            catch(Exception exception)
            {
                Debug.LogError("Charso save failed: "+exception.Message);
                return false;
            }
            return true;
        }

        static PlayerProgress Clean(PlayerProgress p)
        {
            p.LevelIndex=Mathf.Clamp(p.LevelIndex,0,WordPuzzle.Levels.Length-1);
            p.Coins=Mathf.Clamp(p.Coins,0,999999);
            p.RestorationStage=Mathf.Clamp(p.RestorationStage,0,3);
            p.SolvedLevels=Mathf.Clamp(p.SolvedLevels,0,WordPuzzle.Levels.Length);
            p.SchemaVersion=3;
            if(p.AppliedTransactionIds==null)p.AppliedTransactionIds=Array.Empty<string>();
            if(p.OwnedCityProjectIds==null)p.OwnedCityProjectIds=Array.Empty<string>();
            if(p.LastCompletedPuzzleId==null)p.LastCompletedPuzzleId=string.Empty;
            return p;
        }
    }
}