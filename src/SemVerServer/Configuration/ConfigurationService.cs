using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SemVerServer.Configuration
{
    public static class ConfigurationService
    {
        private const string ConfigurationSectionName = "SemVerSection";
        private static IEnumerable<ScopeElement> _scopes;

        public static IEnumerable<ScopeElement> GetConfiguredScopes()
        {
            if (_scopes != null)
            {
                return _scopes;
            }

            var section = (SemVerSection)ConfigurationManager.GetSection(ConfigurationSectionName);

            if (section == null || section.Scopes == null)
            {
                _scopes = Enumerable.Empty<ScopeElement>();
            }
            else
            {
                _scopes = section.Scopes.OfType<ScopeElement>().ToList();
            }

            return _scopes;
        }
    }
}