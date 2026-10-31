using System;
using System.IO;
using UnityEngine;

namespace Charso.Core
{
    public sealed class SaveRepository
    {
        readonly string mainPath;
        readonly string backupPath;
        readonly string tempPath;

        public SaveRepository(string directory, string fileName = "charso-save-v2.json")
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentException("A save directory is required.", nameof(directory));
            mainPath = Path.Combine(directory, fileName);
            backupPath = mainPath + ".bak";
            tempPath = mainPath + ".tmp";
        }

        public SaveGameData LoadOrNew(out bool recoveredFromBackup, out bool migrated)
        {
            recoveredFromBackup = false;
            migrated = false;
            if (TryRead(mainPath, out SaveGameData data, out bool didMigrate))
            {
                migrated = didMigrate;
                return data;
            }
            if (TryRead(backupPath, out data, out didMigrate))
            {
                recoveredFromBackup = true;
                migrated = didMigrate;
                return data;
            }
            return new SaveGameData();
        }

        public void Save(SaveGameData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string directory = Path.GetDirectoryName(mainPath);
            Directory.CreateDirectory(directory);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(SaveCodec.Encode(data));
            using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush(true);
            }

            // Keep the last known good copy. If the process stops between these operations,
            // LoadOrNew can recover from the backup on the next launch.
            if (File.Exists(mainPath)) File.Copy(mainPath, backupPath, true);
            if (File.Exists(mainPath)) File.Delete(mainPath);
            File.Move(tempPath, mainPath);
        }

        static bool TryRead(string path, out SaveGameData data, out bool migrated)
        {
            data = null;
            migrated = false;
            try
            {
                if (!File.Exists(path)) return false;
                string json = File.ReadAllText(path, System.Text.Encoding.UTF8);
                return SaveCodec.TryDecode(json, out data, out migrated);
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Charso save read failed: " + exception.Message);
                return false;
            }
        }
    }
}