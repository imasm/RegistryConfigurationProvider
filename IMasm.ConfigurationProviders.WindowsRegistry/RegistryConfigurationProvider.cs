using Microsoft.Extensions.Configuration;

namespace IMasm.ConfigurationProviders.WindowsRegistry
{
    public class RegistryConfigurationProvider : ConfigurationProvider
    {
        public RegistryConfigurationSource Source { get; }


        public RegistryConfigurationProvider(RegistryConfigurationSource source)
        {
            this.Source = source;
        }

        public override void Load()
        {
            RegisterExplorer explorer = new RegisterExplorer(Source.RegistryHive, Source.RootKey);
            this.Data = explorer.ReadAll();

        }

        public override void Set(string key, string? value)
        {
            base.Set(key, value);
        }
    }
}