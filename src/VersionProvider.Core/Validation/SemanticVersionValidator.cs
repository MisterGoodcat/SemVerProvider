using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VersionProvider.Core.Validation
{
    public class SemanticVersionValidator : ISemanticVersionValidator
    {
        private readonly Regex _allowedAlphaNumerics = new Regex(@"^[0-9A-Za-z-\.]+$");

        public ValidationResult Validate(SemanticVersionInfo versionInfo)
        {
            if (versionInfo.Major < 0)
            {
                return new ValidationResult(false, "Major version must not be negative.");
            }

            if (versionInfo.Minor < 0)
            {
                return new ValidationResult(false, "Minor version must not be negative.");
            }

            if (versionInfo.Patch < 0)
            {
                return new ValidationResult(false, "Patch version must not be negative.");
            }

            var preReleaseError = GetPreReleaseValidationError(versionInfo);
            if (preReleaseError != null)
            {
                return new ValidationResult(false, preReleaseError);   
            }

            var buildMetadataError = GetBuildMetadataValidationError(versionInfo);
            if (buildMetadataError != null)
            {
                return new ValidationResult(false, buildMetadataError);
            }

            return new ValidationResult(true, null);
        }

        private string GetPreReleaseValidationError(SemanticVersionInfo versionInfo)
        {
            var preReleaseIdentifier = versionInfo.PreReleaseIdentifier;
            if (string.IsNullOrWhiteSpace(preReleaseIdentifier))
            {
                return null;
            }

            if (preReleaseIdentifier.StartsWith("-"))
            {
                preReleaseIdentifier = preReleaseIdentifier.Substring(1);
            }

            return GetAlphaNumericsValidationError(preReleaseIdentifier, true);
        }

        private string GetBuildMetadataValidationError(SemanticVersionInfo versionInfo)
        {
            var buildMetadata = versionInfo.BuildMetadata;
            if (string.IsNullOrWhiteSpace(buildMetadata))
            {
                return null;
            }

            if (buildMetadata.StartsWith("+"))
            {
                buildMetadata = buildMetadata.Substring(1);
            }

            return GetAlphaNumericsValidationError(buildMetadata, false);
        }

        private string GetAlphaNumericsValidationError(string content, bool checkForLeadingZeros)
        {
            if (!_allowedAlphaNumerics.IsMatch(content))
            {
                return "Only digits, characters, hyphens and dots are allowed outside the major, minor and patch level parts of a version.";
            }

            var parts = content.Split('.');
            if (parts.Any(string.IsNullOrWhiteSpace))
            {
                return string.Format(CultureInfo.InvariantCulture, "The dot-separated parts of a textual version part cannot be empty ({0})", content);
            }

            if (!checkForLeadingZeros)
            {
                return null;
            }

            foreach (var part in parts)
            {
                var isAllDigits = part.All(char.IsDigit);
                if (isAllDigits && part.StartsWith("0"))
                {
                    return string.Format(CultureInfo.InvariantCulture, "Numeric identifiers must not include leading zeros ({0} in {1})", part, content);
                }
            }

            return null;
        }
    }
}