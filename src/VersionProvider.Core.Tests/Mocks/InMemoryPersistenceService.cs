using VersionProvider.Core.Persistence;

namespace VersionProvider.Core.Tests.Mocks
{
    internal class InMemoryPersistenceService : ISemanticVersionPersistenceService
    {
        private SemanticVersionStorageInfo _storageInfo;

        public SemanticVersionStorageInfo GetPersistedSemanticVersionInfo()
        {
            return _storageInfo ?? (_storageInfo = new SemanticVersionStorageInfo { VersionInfo = new SemanticVersionInfo() });
        }

        public void PersistSemanticVersionInfo(SemanticVersionStorageInfo semanticVersionStorageInfo)
        {
            _storageInfo = semanticVersionStorageInfo;
        }
    }
}