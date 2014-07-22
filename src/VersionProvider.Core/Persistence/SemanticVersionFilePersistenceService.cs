using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace VersionProvider.Core.Persistence
{
    public class SemanticVersionFilePersistenceService : ISemanticVersionPersistenceService
    {
        private readonly string _filePath;
        private static readonly object Locker = new object();

        public SemanticVersionFilePersistenceService(string filePath)
        {
            _filePath = filePath;
        }

        public SemanticVersionStorageInfo GetPersistedSemanticVersionInfo()
        {
            lock (Locker)
            {
                if (!File.Exists(_filePath))
                {
                    return CreateNewStorageInfo();
                }

                var rawContent = File.ReadAllText(_filePath, Encoding.UTF8);
                var result = JsonConvert.DeserializeObject<SemanticVersionStorageInfo>(rawContent);
                return result;
            }
        }

        private SemanticVersionStorageInfo CreateNewStorageInfo()
        {
            var versionInfo = new SemanticVersionInfo();
            var storageInfo = new SemanticVersionStorageInfo
            {
                VersionInfo = versionInfo,
                StorageDate = DateTime.UtcNow,
                ConcurrencyToken = Guid.NewGuid()
            };

            return storageInfo;
        }

        public void PersistSemanticVersionInfo(SemanticVersionStorageInfo semanticVersionStorageInfo)
        {
            lock (Locker)
            {
                if (File.Exists(_filePath))
                {
                    var currentStorageInfo = GetPersistedSemanticVersionInfo();
                    if (currentStorageInfo.ConcurrencyToken != semanticVersionStorageInfo.ConcurrencyToken)
                    {
                        var message = string.Format(
                            CultureInfo.InvariantCulture,
                            "The version info was concurrently changed at {0}.{1} to: '{2}'",
                            currentStorageInfo.StorageDate.ToLongTimeString(),
                            currentStorageInfo.StorageDate.Millisecond,
                            currentStorageInfo.VersionInfo.FormattedVersion);
                        throw new ConcurrentPersistenceException(message);
                    }
                }

                semanticVersionStorageInfo.StorageDate = DateTime.UtcNow;
                semanticVersionStorageInfo.ConcurrencyToken = Guid.NewGuid();

                var rawContent = JsonConvert.SerializeObject(semanticVersionStorageInfo);
                File.WriteAllText(_filePath, rawContent, Encoding.UTF8);
            }
        }
    }
}