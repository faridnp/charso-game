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
        public void SchemaV2EnvelopeMigratesAndKeepsCurrencyAndTransactions()
        {
            var previous = new LegacySaveV2ForTest
            {
                SchemaVersion = 2, LevelIndex = 12, SoftCurrency = 345,
                RestorationStage = 2, SolvedLevels = 12,
                AppliedTransactionIds = new[] { "puzzle:12:solve" }
            };
            string payload = UnityEngine.JsonUtility.ToJson(previous);
            string checksum = Crc32ForTest(payload);
            string envelope = UnityEngine.JsonUtility.ToJson(new SaveEnvelopeForTest
                { FormatVersion = 1, Payload = payload, Checksum = checksum });
            Assert.IsTrue(SaveCodec.TryDecode(envelope, out SaveGameData loaded, out bool migrated));
            Assert.IsTrue(migrated);
            Assert.AreEqual(3, loaded.SchemaVersion);
            Assert.AreEqual(345, loaded.SoftCurrency);
            Assert.AreEqual("puzzle:12:solve", loaded.AppliedTransactionIds[0]);
            Assert.AreEqual(0, loaded.OwnedCityProjectIds.Length);
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
            Assert.AreEqual(3, migratedData.SchemaVersion);
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
    [System.Serializable] public sealed class LegacySaveV2ForTest { public int SchemaVersion=2; public int LevelIndex; public int SoftCurrency; public int RestorationStage; public int SolvedLevels; public bool CurrentLevelSolved; public bool CurrentLevelRestored; public string LastCompletedPuzzleId=""; public string[] AppliedTransactionIds=System.Array.Empty<string>(); }
    [System.Serializable] public sealed class SaveEnvelopeForTest { public int FormatVersion=1; public string Payload; public string Checksum; }
    static string Crc32ForTest(string payload) { uint crc=0xffffffff; byte[] bytes=System.Text.Encoding.UTF8.GetBytes(payload); for(int i=0;i<bytes.Length;i++){crc^=bytes[i];for(int b=0;b<8;b++)crc=(crc&1)!=0?0xedb88320^(crc>>1):crc>>1;} return (~crc).ToString("X8"); }

    }
}