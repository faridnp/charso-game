using System;
using System.IO;
using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class SaveRepositoryTests
    {
        string directory;
        [SetUp]
        public void SetUp()
        {
            directory=Path.Combine(Path.GetTempPath(),"CharsoTests",Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
        }

        [TearDown]
        public void TearDown()
        {
            if(Directory.Exists(directory))Directory.Delete(directory,true);
        }

        [Test]
        public void LoadOrNew_RecoversLastKnownGoodBackup()
        {
            var repository=new SaveRepository(directory,"test-save.json");
            repository.Save(new SaveGameData{LevelIndex=1,SoftCurrency=31});
            repository.Save(new SaveGameData{LevelIndex=2,SoftCurrency=42});
            File.WriteAllText(Path.Combine(directory,"test-save.json"),"{broken");

            SaveGameData loaded=repository.LoadOrNew(out bool recovered,out bool migrated);
            Assert.That(recovered,Is.True);
            Assert.That(migrated,Is.False);
            Assert.That(loaded.LevelIndex,Is.EqualTo(1));
            Assert.That(loaded.SoftCurrency,Is.EqualTo(31));
        }

        [Test]
        public void LoadOrNew_MigratesLegacyV1Save()
        {
            string legacy="{\"SchemaVersion\":1,\"LevelIndex\":2,\"Coins\":47,\"RestorationStage\":1,\"SolvedLevels\":2,\"CurrentLevelSolved\":true,\"CurrentLevelRestored\":false}";
            File.WriteAllText(Path.Combine(directory,"legacy.json"),legacy);
            var repository=new SaveRepository(directory,"legacy.json");

            SaveGameData loaded=repository.LoadOrNew(out bool recovered,out bool migrated);
            Assert.That(recovered,Is.False);
            Assert.That(migrated,Is.True);
            Assert.That(loaded.SchemaVersion,Is.EqualTo(2));
            Assert.That(loaded.SoftCurrency,Is.EqualTo(47));
            Assert.That(loaded.LevelIndex,Is.EqualTo(2));
        }
    }
}