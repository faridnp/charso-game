using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class SaveCodecTests
    {
        [Test]
        public void EncodedSaveRoundTrips()
        {
            var original = new SaveGameData { LevelIndex = 7, SoftCurrency = 123, RestorationStage = 2, SolvedLevels = 6 };
            string json = SaveCodec.Encode(original);
            Assert.IsTrue(SaveCodec.TryDecode(json, out SaveGameData loaded, out bool migrated));
            Assert.IsFalse(migrated);
            Assert.AreEqual(7, loaded.LevelIndex);
            Assert.AreEqual(123, loaded.SoftCurrency);
            Assert.AreEqual(2, loaded.RestorationStage);
        }

        [Test]
        public void CorruptedPayloadIsRejected()
        {
            string json = SaveCodec.Encode(new SaveGameData { SoftCurrency = 10 });
            json = json.Replace("\"SoftCurrency\":10", "\"SoftCurrency\":99");
            Assert.IsFalse(SaveCodec.TryDecode(json, out _, out _));
        }

        [Test]
        public void LegacyV1SaveMigrates()
        {
            const string legacy = "{\"SchemaVersion\":1,\"LevelIndex\":3,\"Coins\":42,\"RestorationStage\":1,\"SolvedLevels\":2,\"CurrentLevelSolved\":true,\"CurrentLevelRestored\":false}";
            Assert.IsTrue(SaveCodec.TryDecode(legacy, out SaveGameData migratedData, out bool migrated));
            Assert.IsTrue(migrated);
            Assert.AreEqual(2, migratedData.SchemaVersion);
            Assert.AreEqual(42, migratedData.SoftCurrency);
            Assert.AreEqual(3, migratedData.LevelIndex);
            Assert.IsTrue(migratedData.CurrentLevelSolved);
        }

        [Test]
        public void NegativeValuesAreClamped()
        {
            const string legacy = "{\"SchemaVersion\":1,\"LevelIndex\":-9,\"Coins\":-20}";
            Assert.IsTrue(SaveCodec.TryDecode(legacy, out SaveGameData data, out _));
            Assert.AreEqual(0, data.LevelIndex);
            Assert.AreEqual(0, data.SoftCurrency);
        }
    }
}