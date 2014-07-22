using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionProvider.Core.Validation;

namespace VersionProvider.Core.Tests
{
    [TestClass]
    public class SemanticVersionValidatorTests
    {
        [TestMethod]
        public void InitializeEmptyVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(), true);
        }

        [TestMethod]
        public void InitializeOnlyPatchLevelVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(0, 0, 1, null, null), true);
        }

        [TestMethod]
        public void InitializeOnlyMinorLevelVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(0, 2, 0, null, null), true);
        }

        [TestMethod]
        public void InitializeOnlyMajorLevelVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(3, 0, 0, null, null), true);
        }

        [TestMethod]
        public void InitializeMajorMinorVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 0, null, null), true);
        }
        
        [TestMethod]
        public void InitializeMajorPatchVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(2, 0, 1, null, null), true);
        }

        [TestMethod]
        public void InitializeMinorPatchVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(0, 2, 5, null, null), true);
        }
        
        [TestMethod]
        public void InitializeValidPreReleaseVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha", null), true);
        }

        [TestMethod]
        public void InitializeValidPreReleaseVersionLeadingHyphen_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "-alpha", null), true);
        }

        [TestMethod]
        public void InitializeValidPreReleaseVersionWithHyphens_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha-1-b", null), true);
        }

        [TestMethod]
        public void InitializeValidPreReleaseVersionWithDots_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha.1.5.first.release", null), true);
        }

        [TestMethod]
        public void InitializeValidPreReleaseVersionWithHyphensAndDots_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha-1.2-second.release", null), true);
        }
        
        [TestMethod]
        public void InitializeValidBuildMetadataVersion_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build42"), true);
        }

        [TestMethod]
        public void InitializeValidBuildMetadataVersionLeadingPlus_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "+build42"), true);
        }

        [TestMethod]
        public void InitializeValidBuildMetadataVersionWithHyphens_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build-1-b"), true);
        }

        [TestMethod]
        public void InitializeValidBuildMetadataVersionWithDots_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build.1.5.first.of.month"), true);
        }

        [TestMethod]
        public void InitializeValidBuildMetadataVersionWithHyphensAndDots_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build42-1.2-second.attempt"), true);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithLeadingZeroIdentifier_ShouldBeValid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build.033.test"), true);
        }

        [TestMethod]
        public void InitializeMajorVersionWithNegativeNumber_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(-42, 0, 0, null, null), false);
        }

        [TestMethod]
        public void InitializeMinorVersionWithNegativeNumber_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(0, -42, 0, null, null), false);
        }

        [TestMethod]
        public void InitializePatchVersionWithNegativeNumber_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(0, 0, -42, null, null), false);
        }
        
        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters1_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp%ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters2_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alphä", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters3_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp_ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters4_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp^ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters5_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp#ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters6_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters7_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp|ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters8_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp~ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters9_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp'ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithInvalidCharacters10_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alp:ha", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithLeadingZeroIdentifier_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha.033.test", null), false);
        }

        [TestMethod]
        public void InitializePreReleaseVersionWithEmptyIdentifier_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, "alpha..test", null), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters1_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp%ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters2_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alphä"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters3_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp_ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters4_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp^ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters5_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp#ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters6_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters7_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp|ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters8_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp~ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters9_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp'ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithInvalidCharacters10_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "alp:ha"), false);
        }

        [TestMethod]
        public void InitializeBuildMetadataVersionWithEmptyIdentifier_ShouldBeInvalid()
        {
            ExecuteValidation(new SemanticVersionInfo(1, 2, 3, null, "build..test"), false);
        }

        private static void ExecuteValidation(SemanticVersionInfo versionInfo, bool expectedAssertionResult)
        {
            var validator = new SemanticVersionValidator();
            var result = validator.Validate(versionInfo);
            Assert.AreEqual(expectedAssertionResult, result.IsValid);
        }
    }
}
