using System;

namespace VersionProvider.Core.Persistence
{
    public class SemanticVersionStorageInfo
    {
        public SemanticVersionInfo VersionInfo { get; set; }
        public Guid ConcurrencyToken { get; set; }
        public DateTime StorageDate { get; set; }
    }
}