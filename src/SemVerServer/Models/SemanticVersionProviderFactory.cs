using System;
using System.Linq;
using System.Web.Hosting;
using SemVerServer.Configuration;
using VersionProvider.Core;
using VersionProvider.Core.Persistence;
using VersionProvider.Core.Validation;

namespace SemVerServer.Models
{
    internal class SemanticVersionProviderFactory : ISemanticVersionProviderFactory
    {
        public ISemanticVersionProvider CreateProvider(string scope)
        {
            var configuredScopes = ConfigurationService.GetConfiguredScopes();
            var scopeElement = configuredScopes.FirstOrDefault(x => x.Name.Equals(scope, StringComparison.OrdinalIgnoreCase));
            if (scopeElement == null)
            {
                return null;
            }

            var trueFilePath = MapFilePath(scopeElement.PathToVersionFile);
            var persistenceService = new SemanticVersionFilePersistenceService(trueFilePath);
            var validator = new SemanticVersionValidator();
            return new SemanticVersionProvider(persistenceService, validator);
        }

        private string MapFilePath(string filePath)
        {
            if (filePath.StartsWith("~"))
            {
                return HostingEnvironment.MapPath(filePath);
            }

            return filePath;
        }
    }
}