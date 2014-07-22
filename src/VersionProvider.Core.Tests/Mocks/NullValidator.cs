using VersionProvider.Core.Validation;

namespace VersionProvider.Core.Tests.Mocks
{
    internal class NullValidator : ISemanticVersionValidator
    {
        public ValidationResult Validate(SemanticVersionInfo versionInfo)
        {
            return new ValidationResult(true, null);
        }
    }
}