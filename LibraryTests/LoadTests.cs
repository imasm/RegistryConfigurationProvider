using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using IMasm.ConfigurationProviders.WindowsRegistry;

namespace LibraryTests
{
    public class LoadTests
    {
        private readonly string _rootKey = @"SOFTWARE\imasm\RegistryConfigurationTests";

        private const string KeyA = "KeyA";
        private const string KeyB = "KeyB";
        private const string KeyC = "KeyC";

        private const string BaseStringValue = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
        private const string KeyAStringValue = "Key A -Lorem ipsum dolor sit amet";
        private const string KeyBStringValue = "Key B -Lorem ipsum dolor sit amet";
        private const string KeyCStringValue = "Key C -Lorem ipsum dolor sit amet";
        private const string ValueName = "TheValue";


        public LoadTests()
        {

            this.Initialize();
        }

        private void Initialize()
        {
            // String in root
            using var regKey = Registry.CurrentUser.CreateSubKey(_rootKey, true, RegistryOptions.Volatile);
            regKey.SetValue(ValueName, BaseStringValue, RegistryValueKind.String);

            // String in subkeys
            using var keyA = regKey.CreateSubKey(KeyA, true, RegistryOptions.Volatile);
            keyA.SetValue(ValueName, KeyAStringValue, RegistryValueKind.String);

            using var keyB = regKey.CreateSubKey(KeyB, true, RegistryOptions.Volatile);
            keyB.SetValue(ValueName, KeyBStringValue, RegistryValueKind.String);

            // Nested subkey
            using var keyC = keyA.CreateSubKey(KeyC, true, RegistryOptions.Volatile);
            keyC.SetValue(ValueName, KeyCStringValue, RegistryValueKind.String);
        }

        [Fact]
        public void TestBuildConfig()
        {

            var configurationRoot = new ConfigurationBuilder()
                .AddWindowsRegistry(_rootKey, RegistryHive.CurrentUser)
                .Build();

            Assert.NotNull(configurationRoot);

        }

        [Fact]
        public void TestLoadValues()
        {

            var configurationRoot = new ConfigurationBuilder()
                .AddWindowsRegistry(_rootKey, RegistryHive.CurrentUser)
                .Build();

            Assert.NotNull(configurationRoot);
            var baseString = configurationRoot[ValueName];
            Assert.Equal(BaseStringValue, baseString);

            var keyAString = configurationRoot[$"{KeyA}:{ValueName}"];
            Assert.Equal(KeyAStringValue, keyAString);

            var keyBString = configurationRoot[$"{KeyB}:{ValueName}"];
            Assert.Equal(KeyBStringValue, keyBString);

            var keyCString = configurationRoot[$"{KeyA}:{KeyC}:{ValueName}"];
            Assert.Equal(KeyCStringValue, keyCString);
        }


        [Fact]
        public void TestReload()
        {
            // String in root
            using var regKey = Registry.CurrentUser.CreateSubKey(_rootKey, true, RegistryOptions.Volatile);
            regKey.SetValue("Test", "Test1", RegistryValueKind.String);

            var configurationRoot = new ConfigurationBuilder()
                .AddWindowsRegistry(_rootKey, RegistryHive.CurrentUser)
                .Build();

            var testString = configurationRoot["Test"];
            Assert.Equal("Test1", testString);


            regKey.SetValue("Test", "TestXXXX", RegistryValueKind.String);
            configurationRoot.Reload();

            testString = configurationRoot["Test"];
            Assert.Equal("TestXXXX", testString);
        }
    }
}