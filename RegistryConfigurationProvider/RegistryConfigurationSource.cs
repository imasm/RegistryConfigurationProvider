using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System.Globalization;

namespace RegistryConfigurationProvider
{
    public class RegistryConfigurationSource : IConfigurationSource
    {
        public RegistryHive RegistryHive { get; }
        public string RootKey { get; }

        public RegistryConfigurationSource(string rootKey) : this(RegistryHive.CurrentUser, rootKey) { }

        public RegistryConfigurationSource(RegistryHive registryHive, string rootKey)
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