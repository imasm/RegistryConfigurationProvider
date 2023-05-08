using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace IMasm.ConfigurationProviders.WindowsRegistry
{
    public static class RegistryConfigurationExtensions
    {
        public static IConfigurationBuilder AddWindowsRegistry(this IConfigurationBuilder builder,
            string rootKey, RegistryHive registryHive = RegistryHive.CurrentUser)
        {
            var source = new RegistryConfigurationSource(rootKey, registryHive);
            return builder.Add(source);
        }
    }
}