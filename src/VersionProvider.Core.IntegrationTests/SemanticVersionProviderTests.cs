using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionProvider.Core.Persistence;
using VersionProvider.Core.Validation;

namespace VersionProvider.Core.IntegrationTests
{
    [TestClass]
    public class SemanticVersionProviderTests
    {
        private readonly string _filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        private SemanticVersionProvider _versionProvider;

        [TestInitialize]
        public void InitializeTest()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var validator = new SemanticVersionValidator();
            _versionProvider = new SemanticVersionProvider(persistenceService, validator);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _versionProvider = null;

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
        
        [TestMethod]
        public void GetCurrentVersionForFirstTime_ReturnsCorrectVersion()
        {
            var version = _versionProvider.GetCurrentVersion();
            Assert.AreEqual("0.1.0", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMajorVersion_ReturnsCorrectVersion()
        {
            var version = _versionProvider.GetNextBreakingChangesVersion();
            Assert.AreEqual("1.0.0", version.FormattedVersion);
        }
        
        [TestMethod]
        public void GetNextMinorVersion_ReturnsCorrectVersion()
        {
            var version = _versionProvider.GetNextFeatureVersion();
            Assert.AreEqual("0.2.0", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPatchVersion_ReturnsCorrectVersion()
        {
            var version = _versionProvider.GetNextBugFixVersion();
            Assert.AreEqual("0.1.1", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMajorVersionMultipleTimes_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion();
            _versionProvider.GetNextBreakingChangesVersion();
            _versionProvider.GetNextBreakingChangesVersion();
            var version = _versionProvider.GetNextBreakingChangesVersion();
            Assert.AreEqual("4.0.0", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextMinorVersionMultipleTimes_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextFeatureVersion();
            _versionProvider.GetNextFeatureVersion();
            _versionProvider.GetNextFeatureVersion();
            var version = _versionProvider.GetNextFeatureVersion();
            Assert.AreEqual("0.5.0", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextPatchVersionMultipleTimes_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBugFixVersion();
            _versionProvider.GetNextBugFixVersion();
            _versionProvider.GetNextBugFixVersion();
            _versionProvider.GetNextBugFixVersion();
            var version = _versionProvider.GetNextBugFixVersion();
            Assert.AreEqual("0.1.5", version.FormattedVersion);
        }
        
        [TestMethod]
        public void GetMultipleNextVersions1_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBugFixVersion(); // 0.1.1
            _versionProvider.GetNextBugFixVersion(); // 0.1.2
            var version = _versionProvider.GetNextFeatureVersion(); // reset to 0.2.0
            
            Assert.AreEqual("0.2.0", version.FormattedVersion);
        }

        [TestMethod]
        public void GetMultipleNextVersions2_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBugFixVersion(); // 0.1.1
            _versionProvider.GetNextBugFixVersion(); // 0.1.2
            _versionProvider.GetNextFeatureVersion(); // reset to 0.2.0
            _versionProvider.GetNextFeatureVersion(); // 0.3.0
            _versionProvider.GetNextFeatureVersion(); // 0.4.0
            var version = _versionProvider.GetNextBugFixVersion(); // 0.4.1

            Assert.AreEqual("0.4.1", version.FormattedVersion);
        }

        [TestMethod]
        public void GetMultipleNextVersions3_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion(); // 1.1.0
            _versionProvider.GetNextBugFixVersion(); // 1.1.1
            _versionProvider.GetNextBreakingChangesVersion(); // 2.0.0
            var version = _versionProvider.GetNextBugFixVersion(); // 2.0.1

            Assert.AreEqual("2.0.1", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithPreReleaseIdentifier1_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion("alpha-1", null);

            Assert.AreEqual("1.1.0-alpha-1", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithPreReleaseIdentifier2_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion("-alpha-1", null);

            Assert.AreEqual("1.1.0-alpha-1", version.FormattedVersion);
        }
        
        [TestMethod]
        public void GetNextVersionsWithPreReleaseIdentifier3_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion("alpha.12.b", null);

            Assert.AreEqual("1.1.0-alpha.12.b", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithBuildMetadata1_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion(null, "build-42");

            Assert.AreEqual("1.1.0+build-42", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithBuildMetadata2_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion(null, "+build-42");

            Assert.AreEqual("1.1.0+build-42", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithBuildMetadata3_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion(null, "build.42-b");

            Assert.AreEqual("1.1.0+build.42-b", version.FormattedVersion);
        }

        [TestMethod]
        public void GetNextVersionsWithPreReleaseIdentifierAndBuildMetadata1_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion("beta.4.a", "build-42");

            Assert.AreEqual("1.1.0-beta.4.a+build-42", version.FormattedVersion);
        }
        
        [TestMethod]
        public void GetNextVersionsWithBuildMetadataThatIsInvalidPreReleaseIdentifier_ReturnsCorrectVersion()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            var version = _versionProvider.GetNextFeatureVersion(null, "build.02.b");

            Assert.AreEqual("1.1.0+build.02.b", version.FormattedVersion);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersionException))]
        public void GetNextVersionsWithInvalidPreReleaseIdentifier1_ThrowsInvalidVersionException()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion("alpha.01.b", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersionException))]
        public void GetNextVersionsWithInvalidPreReleaseIdentifier2_ThrowsInvalidVersionException()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion("alpha..b", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersionException))]
        public void GetNextVersionsWithInvalidPreReleaseIdentifier3_ThrowsInvalidVersionException()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion("alphä", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersionException))]
        public void GetNextVersionsWithInvalidBuildMetadata1_ThrowsInvalidVersionException()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion(null, "Build..2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersionException))]
        public void GetNextVersionsWithInvalidBuildMetadata2_ThrowsInvalidVersionException()
        {
            _versionProvider.GetNextBreakingChangesVersion(); // 1.0.0
            _versionProvider.GetNextFeatureVersion(null, "Bu!ld");
        }
    }
}
