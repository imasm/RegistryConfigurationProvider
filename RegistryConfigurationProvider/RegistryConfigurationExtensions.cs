using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace RegistryConfigurationProvider
{
    public static class RegistryConfigurationExtensions
    {
        public static IConfigurationBuilder AddWindowsRegistry(this IConfigurationBuilder builder,
            RegistryHive registryHive, string rootKey)
        {
            var source = new RegistryConfigurationSource(registryHive, rootKey);
            return builder.Add(source);
        }
    }
}