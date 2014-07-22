using System;
using System.ComponentModel.DataAnnotations;
using VersionProvider.Core.Persistence;
using VersionProvider.Core.Validation;

namespace VersionProvider.Core
{
    public class SemanticVersionProvider : ISemanticVersionProvider
    {
        private readonly ISemanticVersionPersistenceService _semanticVersionPersistenceService;
        private readonly ISemanticVersionValidator _semanticVersionValidator;

        public SemanticVersionProvider(ISemanticVersionPersistenceService semanticVersionPersistenceService, ISemanticVersionValidator semanticVersionValidator)
        {
            _semanticVersionPersistenceService = semanticVersionPersistenceService;
            _semanticVersionValidator = semanticVersionValidator;
        }

        public SemanticVersionInfo GetCurrentVersion()
        {
            return _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo().VersionInfo;
        }

        public SemanticVersionInfo GetNextBugFixVersion()
        {
            return GetNextBugFixVersion(null, null);
        }

        public SemanticVersionInfo GetNextFeatureVersion()
        {
            return GetNextFeatureVersion(null, null);
        }

        public SemanticVersionInfo GetNextBreakingChangesVersion()
        {
            return GetNextBreakingChangesVersion(null, null);
        }

        public SemanticVersionInfo GetNextPreRelaseVersion(string preReleaseIdentifier, string buildMetadata)
        {
            var storageInfo = _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo();
            var versionInfo = storageInfo.VersionInfo;
            SetCommonData(versionInfo, preReleaseIdentifier, buildMetadata);

            ThrowIfNotValid(versionInfo);

            _semanticVersionPersistenceService.PersistSemanticVersionInfo(storageInfo);
            return versionInfo;
        }

        public SemanticVersionInfo GetNextBugFixVersion(string preReleaseIdentifier, string buildMetadata)
        {
            var storageInfo = _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo();
            var versionInfo = storageInfo.VersionInfo;
            versionInfo.Patch += 1;
            SetCommonData(versionInfo, preReleaseIdentifier, buildMetadata);

            ThrowIfNotValid(versionInfo);

            _semanticVersionPersistenceService.PersistSemanticVersionInfo(storageInfo);
            return versionInfo;
        }

        public SemanticVersionInfo GetNextFeatureVersion(string preReleaseIdentifier, string buildMetadata)
        {
            var storageInfo = _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo();
            var versionInfo = storageInfo.VersionInfo;
            versionInfo.Minor += 1;
            versionInfo.Patch = 0;
            SetCommonData(versionInfo, preReleaseIdentifier, buildMetadata);

            ThrowIfNotValid(versionInfo);

            _semanticVersionPersistenceService.PersistSemanticVersionInfo(storageInfo);
            return versionInfo;
        }

        public SemanticVersionInfo GetNextBreakingChangesVersion(string preReleaseIdentifier, string buildMetadata)
        {
            var storageInfo = _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo();
            var versionInfo = storageInfo.VersionInfo;
            versionInfo.Major += 1;
            versionInfo.Minor = 0;
            versionInfo.Patch = 0;
            SetCommonData(versionInfo, preReleaseIdentifier, buildMetadata);
            
            ThrowIfNotValid(versionInfo);

            _semanticVersionPersistenceService.PersistSemanticVersionInfo(storageInfo);
            return versionInfo;
        }

        public void SetVersion(SemanticVersionInfo versionInfo)
        {
            ThrowIfNotValid(versionInfo);

            var storageInfo = _semanticVersionPersistenceService.GetPersistedSemanticVersionInfo();
            storageInfo.VersionInfo = versionInfo;

            _semanticVersionPersistenceService.PersistSemanticVersionInfo(storageInfo);
        }

        private static void SetCommonData(SemanticVersionInfo versionInfo, string preReleaseIdentifier, string buildMetadata)
        {
            versionInfo.PreReleaseIdentifier = preReleaseIdentifier;
            versionInfo.BuildMetadata = buildMetadata;
        }

        private void ThrowIfNotValid(SemanticVersionInfo versionInfo)
        {
            var validationResult = _semanticVersionValidator.Validate(versionInfo);
            if (!validationResult.IsValid)
            {
                throw new InvalidVersionException(validationResult.ValidationError);
            }
        }
    }
}