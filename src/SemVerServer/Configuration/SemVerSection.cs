using System.Configuration;

namespace SemVerServer.Configuration
{
    public class SemVerSection : ConfigurationSection
    {
        [ConfigurationProperty("Scopes", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ScopeElement), AddItemName = "Scope")]
        public ScopesCollection Scopes
        {
            get { return (ScopesCollection)this["Scopes"]; }
        }
    }
}