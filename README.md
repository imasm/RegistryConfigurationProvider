# Windows Registry configuration provider implementation

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


## Usage 

```
dotnet add package IMasm.ConfigurationProviders.WindowsRegistry
```

```csharp
 var configurationRoot = new ConfigurationBuilder()
                .AddWindowsRegistry(@"SOFTWARE\CompanyName\ApplicationName", RegistryHive.CurrentUser)
                .Build();
```
