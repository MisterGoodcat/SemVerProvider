namespace VersionProvider.Core.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ValidationError { get; private set; }

        public ValidationResult(bool isValid, string validationError)
        {
            IsValid = isValid;
            ValidationError = validationError;
        }
    }
}