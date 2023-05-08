using Microsoft.Win32;

namespace IMasm.ConfigurationProviders.WindowsRegistry
{
    internal class RegisterExplorer : IDisposable
    {
        private RegistryKey? _regKey;
        private readonly Stack<string> _subKeys;
        private readonly SortedDictionary<string, string?> _data;

        public RegisterExplorer(RegistryHive registryHive, string regPath)
        {
            _subKeys = new Stack<string>();
            _data = new SortedDictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(regPath))
                throw new ArgumentNullException(nameof(regPath));

            _regKey = Open(registryHive, regPath);
        }

        private RegistryKey? Open(RegistryHive registryHive, string rootKey)
        {
            return registryHive switch
            {
                RegistryHive.LocalMachine => Registry.LocalMachine.OpenSubKey(rootKey),
                RegistryHive.CurrentUser => Registry.CurrentUser.OpenSubKey(rootKey),
                _ => throw new NotSupportedException($"{registryHive} is not suported"),
            };
        }

        public IDictionary<string, string?> ReadAll()
        {
            _data.Clear();
            _subKeys.Clear();

            if (_regKey == null)
                return _data;

            _data.Clear();
            Explore(_regKey);
            return _data;
        }

        private string GetDataKey(string valueName)
        {
            var subkeys = _subKeys.ToArray();
            if (subkeys.Length == 0)
                return valueName;

            var path = string.Join(":", subkeys.Reverse());
            return $"{path}:{valueName}";
        }

        private void Explore(RegistryKey regKey)
        {
            // Values
            string[] valueNames = regKey.GetValueNames() ?? Array.Empty<string>();
            if (valueNames.Length > 0)
            {
                foreach (string valueName in valueNames)
                {
                    object? value = regKey.GetValue(valueName);
                    _data.Add(GetDataKey(valueName), value?.ToString());
                }
            }

            // SubKeys
            string[] keyNames = regKey.GetSubKeyNames() ?? Array.Empty<string>();
            if (keyNames.Length > 0)
            {
                foreach (string keyName in keyNames)
                {
                    _subKeys.Push(keyName);
                    using (RegistryKey? subKey = regKey.OpenSubKey(keyName))
                    {
                        if (subKey != null)
                            Explore(subKey);
                    }
                    _subKeys.Pop();
                }
            }
        }

        public void Dispose()
        {
            _regKey?.Dispose();
            _regKey = null;
        }
    }
}