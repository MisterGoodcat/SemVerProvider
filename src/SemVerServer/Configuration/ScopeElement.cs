using System.Configuration;

namespace SemVerServer.Configuration
{
    public class ScopeElement : ConfigurationElement
    {
        [ConfigurationProperty("Id", IsRequired = true)]
        public int Id
        {
            get
            {
                return (int)this["Id"];
            }
        }

        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["Name"];
            }
        }

        [ConfigurationProperty("PathToVersionFile", IsRequired = true)]
        public string PathToVersionFile
        {
            get
            {
                return (string)this["PathToVersionFile"];
            }
        }
    }
}