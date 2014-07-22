namespace VersionProvider.Core.Validation
{
    public interface ISemanticVersionValidator
    {
        ValidationResult Validate(SemanticVersionInfo versionInfo);
    }
}