namespace VersionProvider.Core
{
    public interface ISemanticVersionProvider
    {
        SemanticVersionInfo GetCurrentVersion();
        SemanticVersionInfo GetNextBugFixVersion();
        SemanticVersionInfo GetNextFeatureVersion();
        SemanticVersionInfo GetNextBreakingChangesVersion();
        SemanticVersionInfo GetNextPreRelaseVersion(string preReleaseIdentifier, string buildMetadata);
        SemanticVersionInfo GetNextBugFixVersion(string preReleaseIdentifier, string buildMetadata);
        SemanticVersionInfo GetNextFeatureVersion(string preReleaseIdentifier, string buildMetadata);
        SemanticVersionInfo GetNextBreakingChangesVersion(string preReleaseIdentifier, string buildMetadata);
        void SetVersion(SemanticVersionInfo versionInfo);
    }
}