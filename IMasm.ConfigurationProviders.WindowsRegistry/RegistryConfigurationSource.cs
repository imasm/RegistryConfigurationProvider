using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace IMasm.ConfigurationProviders.WindowsRegistry
{
    public class RegistryConfigurationSource : IConfigurationSource
    {
        public RegistryHive RegistryHive { get; }
        public string RootKey { get; }

        public RegistryConfigurationSource(string rootKey) : this(rootKey, RegistryHive.CurrentUser) { }

        public RegistryConfigurationSource(string rootKey, RegistryHive registryHive)
        {
            this.RegistryHive = registryHive;
            this.RootKey = rootKey;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RegistryConfigurationProvider(this);
        }
    }
}