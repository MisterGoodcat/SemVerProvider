using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VersionProvider.Core.Persistence;

namespace VersionProvider.Core.IntegrationTests
{
    [TestClass]
    public class SemanticVersionFilePersistenceServiceTests
    {
        private readonly string _filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        [TestInitialize]
        public void InitializeTest()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [TestMethod]
        public void GetNonExistingVersionInfo_ReturnsNonEmptyVersionInfo()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var storageInfo = persistenceService.GetPersistedSemanticVersionInfo();

            Assert.IsFalse(SemanticVersionInfo.IsEmpty(storageInfo.VersionInfo));
        }

        [TestMethod]
        public void GetNonExistingVersionInfo_ReturnsInitializedVersionInfo()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var storageInfo = persistenceService.GetPersistedSemanticVersionInfo();

            Assert.AreEqual("0.1.0", storageInfo.VersionInfo.FormattedVersion);
        }

        [TestMethod]
        public void GetNonExistingVersionInfo_ReturnsInitializedStorageDate()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var storageInfo = persistenceService.GetPersistedSemanticVersionInfo();

            Assert.IsFalse(storageInfo.StorageDate == default(DateTime));
        }

        [TestMethod]
        public void StoringNonExistingVersionInfo_CreatesFile()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var storageInfo = persistenceService.GetPersistedSemanticVersionInfo();
            persistenceService.PersistSemanticVersionInfo(storageInfo);

            Assert.IsTrue(File.Exists(_filePath));
        }

        [TestMethod]
        public void StoringAndRetrieveVersionInfo_RetrievesStoredVersionInfo()
        {
            var storageInfo = new SemanticVersionStorageInfo
            {
                VersionInfo = new SemanticVersionInfo(1, 2, 3, "alpha-1", "build.42-b")
            };

            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            persistenceService.PersistSemanticVersionInfo(storageInfo);

            var retrievedStorageInfo = persistenceService.GetPersistedSemanticVersionInfo();

            Assert.AreEqual(storageInfo.VersionInfo.FormattedVersion, retrievedStorageInfo.VersionInfo.FormattedVersion);
        }

        [TestMethod]
        [ExpectedException(typeof(ConcurrentPersistenceException))]
        public void ParallelStorageAttemptsWithNewlyCreatedVersionInfo_ThrowConcurrencyException()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);

            var retrievedStorageInfo1 = persistenceService.GetPersistedSemanticVersionInfo();
            var retrievedStorageInfo2 = persistenceService.GetPersistedSemanticVersionInfo();

            retrievedStorageInfo1.VersionInfo.Major = 10;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo1);

            retrievedStorageInfo2.VersionInfo.Major = 11;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo2);
        }

        [TestMethod]
        [ExpectedException(typeof(ConcurrentPersistenceException))]
        public void ParallelStorageAttemptsWithExistingVersionInfo_ThrowConcurrencyException()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);
            var versionInfo = new SemanticVersionInfo(1, 2, 3, null, null);
            var storageInfo = new SemanticVersionStorageInfo {VersionInfo = versionInfo};
            persistenceService.PersistSemanticVersionInfo(storageInfo);

            var retrievedStorageInfo1 = persistenceService.GetPersistedSemanticVersionInfo();
            var retrievedStorageInfo2 = persistenceService.GetPersistedSemanticVersionInfo();

            retrievedStorageInfo1.VersionInfo.Major = 10;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo1);

            retrievedStorageInfo2.VersionInfo.Major = 11;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo2);
        }

        [TestMethod]
        public void SequentialStorageAttempts_ConcurrencyExceptionNotThrown()
        {
            var persistenceService = new SemanticVersionFilePersistenceService(_filePath);

            var retrievedStorageInfo1 = persistenceService.GetPersistedSemanticVersionInfo();
            retrievedStorageInfo1.VersionInfo.Major = 10;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo1);
            
            var retrievedStorageInfo2 = persistenceService.GetPersistedSemanticVersionInfo();
            retrievedStorageInfo2.VersionInfo.Major = 11;
            persistenceService.PersistSemanticVersionInfo(retrievedStorageInfo2);

            var finalStorageInfo = persistenceService.GetPersistedSemanticVersionInfo();
            Assert.AreEqual(11, finalStorageInfo.VersionInfo.Major);
        }
    }
}
