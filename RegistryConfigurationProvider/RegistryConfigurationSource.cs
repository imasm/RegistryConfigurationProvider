using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace RegistryConfigurationProvider
{
    public class RegistryConfigurationSource : IConfigurationSource
    {
        public RegistryHive RegistryHive { get; }
        public string KeyPath { get; }

        public RegistryConfigurationSource(string keyPath) : this(RegistryHive.CurrentUser, keyPath) { }

        public RegistryConfigurationSource(RegistryHive registryHive, string keyPath)
        {
            this.RegistryHive = registryHive;
            this.KeyPath = keyPath;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RegistryConfigurationProvider(this);
        }
    }
}