namespace VersionProvider.Core.Persistence
{
    public interface ISemanticVersionPersistenceService
    {
        SemanticVersionStorageInfo GetPersistedSemanticVersionInfo();
        void PersistSemanticVersionInfo(SemanticVersionStorageInfo semanticVersionStorageInfo);
    }
}