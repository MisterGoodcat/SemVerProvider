using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionProvider.Core.Tests.Mocks;
using VersionProvider.Core.Validation;

namespace VersionProvider.Core.Tests
{
    [TestClass]
    public class SemanticVersionProviderTests
    {
        [TestMethod]
        public void GetVersionInfoForTheFirstTime_IsSemVerDefault()
        {
            var provider = CreateProvider();
            var versionInfo = provider.GetCurrentVersion();
            Assert.AreEqual("0.1.0", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPatchVersion_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextBugFixVersion();
            Assert.AreEqual("1.2.4", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMinorReleaseVersion_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextFeatureVersion();
            Assert.AreEqual("1.3.0", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMajorReleaseVersion_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextBreakingChangesVersion();
            Assert.AreEqual("2.0.0", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPreReleaseVersion_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextPreRelaseVersion("alpha-2", null);
            Assert.AreEqual("1.2.3-alpha-2", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPreReleaseVersionWithBuildMetadata_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextPreRelaseVersion("alpha-2", "build-44");
            Assert.AreEqual("1.2.3-alpha-2+build-44", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPatchVersionWithPreReleaseIdentifierAndBuildMetadata_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextBugFixVersion("alpha-2", "build-2");
            Assert.AreEqual("1.2.4-alpha-2+build-2", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMinorReleaseVersionWithPreReleaseIdentifierAndBuildMetadata_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextFeatureVersion("alpha-2", "build-2");
            Assert.AreEqual("1.3.0-alpha-2+build-2", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMajorReleaseVersionWithPreReleaseIdentifierAndBuildMetadata_IsCorrectNewVersion()
        {
            var provider = CreateProvider(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build-1"));
            var versionInfo = provider.GetNextBreakingChangesVersion("alpha-2", "build-2");
            Assert.AreEqual("2.0.0-alpha-2+build-2", versionInfo.FormattedVersion);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidMajorVersion_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(-1, 2, 3, "alpha-1", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidMinorVersion_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, -2, 3, "alpha-1", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidPatchVersion_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, -3, "alpha-1", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidPreReleaseVersionWithLeadingZero_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, 3, "alpha.01", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidPreReleaseVersionWithEmptyIdentifier_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, 3, "alpha..4", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidPreReleaseVersionWithInvalidCharacter_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, 3, "alpha&%&1", "build-1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidBuildMetadataVersionWithEmptyIdentifier_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build..1"));
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidVersionException))]
        public void SetInvalidBuildMetadataVersionWithInvalidCharacter_FailsWithException()
        {
            var provider = CreateProviderWithValidator();
            provider.SetVersion(new SemanticVersionInfo(1, 2, 3, "alpha-1", "build:>|-1"));
        }

        private ISemanticVersionProvider CreateProvider(SemanticVersionInfo versionInfo = null)
        {
            var result = new SemanticVersionProvider(new InMemoryPersistenceService(), new NullValidator());

            if (versionInfo != null)
            {
                result.SetVersion(versionInfo);
            }

            return result;
        }

        private ISemanticVersionProvider CreateProviderWithValidator()
        {
            var result = new SemanticVersionProvider(new InMemoryPersistenceService(), new SemanticVersionValidator());
            return result;
        }
    }
}