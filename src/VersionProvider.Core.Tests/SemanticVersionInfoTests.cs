using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VersionProvider.Core.Tests
{
    [TestClass]
    public class SemanticVersionInfoTests
    {
        [TestMethod]
        public void InitializeMajorMinorPatchVersion_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, null, null);
            Assert.AreEqual("1.2.3", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void InitializePreReleaseIdentifierVersion_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, "alpha.2-0.first", null);
            Assert.AreEqual("1.2.3-alpha.2-0.first", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void InitializePreReleaseIdentifierVersionIncludingLeadingHyphen_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, "-alpha.2-0.first", null);
            Assert.AreEqual("1.2.3-alpha.2-0.first", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersion_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, null, "build-4.55");
            Assert.AreEqual("1.2.3+build-4.55", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionIncludingLeadingPlus_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, null, "+build-4.55");
            Assert.AreEqual("1.2.3+build-4.55", versionInfo.FormattedVersion);
        }

        [TestMethod]
        public void InitializePreReleaseAndBuildMetadataVersion_ShouldBeFormattedCorrectly()
        {
            var versionInfo = new SemanticVersionInfo(1, 2, 3, "alpha-1.b-one", "build-42");
            Assert.AreEqual("1.2.3-alpha-1.b-one+build-42", versionInfo.FormattedVersion);
        }
    }
}