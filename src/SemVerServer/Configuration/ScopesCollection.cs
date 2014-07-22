using System.Configuration;

namespace SemVerServer.Configuration
{
    public class ScopesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScopeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScopeElement)element).Id;
        }
    }
}