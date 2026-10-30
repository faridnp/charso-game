using System;
using System.Text;
using UnityEngine;

namespace Charso.Core
{
    public static class SaveCodec
    {
        public static string Encode(SaveGameData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Sanitize(data);
            string payload = JsonUtility.ToJson(data);
            var envelope = new SaveEnvelope
            {
                FormatVersion = 1,
                Payload = payload,
                Checksum = Crc32.Compute(Encoding.UTF8.GetBytes(payload)).ToString("X8")
            };
            return JsonUtility.ToJson(envelope, true);
        }

        public static bool TryDecode(string json, out SaveGameData data, out bool migrated)
        {
            data = null;
            migrated = false;
            if (string.IsNullOrWhiteSpace(json)) return false;

            try
            {
                SaveEnvelope envelope = JsonUtility.FromJson<SaveEnvelope>(json);
                if (envelope != null && !string.IsNullOrEmpty(envelope.Payload))
                {
                    if (envelope.FormatVersion != 1 || string.IsNullOrEmpty(envelope.Checksum)) return false;
                    string expected = Crc32.Compute(Encoding.UTF8.GetBytes(envelope.Payload)).ToString("X8");
                    if (!string.Equals(expected, envelope.Checksum, StringComparison.OrdinalIgnoreCase)) return false;
                    data = JsonUtility.FromJson<SaveGameData>(envelope.Payload);
                    if (data == null || data.SchemaVersion != 2) { data = null; return false; }
                    Sanitize(data);
                    return true;
                }

                LegacySaveV1 legacy = JsonUtility.FromJson<LegacySaveV1>(json);
                if (legacy == null || legacy.SchemaVersion != 1) return false;
                data = new SaveGameData
                {
                    SchemaVersion = 2,
                    LevelIndex = legacy.LevelIndex,
                    SoftCurrency = legacy.Coins,
                    RestorationStage = legacy.RestorationStage,
                    SolvedLevels = legacy.SolvedLevels,
                    CurrentLevelSolved = legacy.CurrentLevelSolved,
                    CurrentLevelRestored = legacy.CurrentLevelRestored
                };
                Sanitize(data);
                migrated = true;
                return true;
            }
            catch (Exception)
            {
                data = null;
                migrated = false;
                return false;
            }
        }

        static void Sanitize(SaveGameData data)
        {
            data.SchemaVersion = 2;
            data.LevelIndex = Math.Max(0, Math.Min(data.LevelIndex, 100000));
            data.SoftCurrency = Math.Max(0, Math.Min(data.SoftCurrency, 999999));
            data.RestorationStage = Math.Max(0, Math.Min(data.RestorationStage, 1000));
            data.SolvedLevels = Math.Max(0, Math.Min(data.SolvedLevels, 100000));
            if (data.AppliedTransactionIds == null) data.AppliedTransactionIds = Array.Empty<string>();
            if (data.LastCompletedPuzzleId == null) data.LastCompletedPuzzleId = string.Empty;
        }
    }

    internal static class Crc32
    {
        static readonly uint[] Table = BuildTable();

        public static uint Compute(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; i++)
                crc = Table[(crc ^ bytes[i]) & 0xff] ^ (crc >> 8);
            return ~crc;
        }

        static uint[] BuildTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < table.Length; i++)
            {
                uint value = i;
                for (int bit = 0; bit < 8; bit++)
                    value = (value & 1) != 0 ? 0xedb88320 ^ (value >> 1) : value >> 1;
                table[i] = value;
            }
            return table;
        }
    }
}