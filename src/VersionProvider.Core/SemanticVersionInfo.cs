using System;
using System.Globalization;
using System.Text;

namespace VersionProvider.Core
{
    public class SemanticVersionInfo
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string PreReleaseIdentifier { get; set; }
        public string BuildMetadata { get; set; }
        
        public string FormattedVersion { get { return ToString(); } }

        public SemanticVersionInfo()
        {
            Major = 0;
            Minor = 1;
            Patch = 0;
            PreReleaseIdentifier = null;
            BuildMetadata = null;
        }

        public SemanticVersionInfo(int major, int minor, int patch, string preReleaseIdentifier, string buildMetadata)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreReleaseIdentifier = preReleaseIdentifier;
            BuildMetadata = buildMetadata;
        }

        public override string ToString()
        {
            var version = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", Major, Minor, Patch);
            var sb = new StringBuilder(version);

            if (!string.IsNullOrWhiteSpace(PreReleaseIdentifier))
            {
                if (!PreReleaseIdentifier.StartsWith("-"))
                {
                    sb.Append("-");
                }

                sb.Append(PreReleaseIdentifier);
            }

            if (!string.IsNullOrWhiteSpace(BuildMetadata))
            {
                if (!BuildMetadata.StartsWith("+"))
                {
                    sb.Append("+");
                }

                sb.Append(BuildMetadata);
            }

            var result = sb.ToString();
            return result;
        }

        public static bool IsEmpty(SemanticVersionInfo versionInfo)
        {
            return versionInfo.Major == 0 && versionInfo.Minor == 0 && versionInfo.Patch == 0 && string.IsNullOrEmpty(versionInfo.PreReleaseIdentifier);
        }
    }
}