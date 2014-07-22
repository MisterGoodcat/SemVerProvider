using VersionProvider.Core;

namespace SemVerServer.Models
{
    internal interface ISemanticVersionProviderFactory
    {
        ISemanticVersionProvider CreateProvider(string scope);
    }
}