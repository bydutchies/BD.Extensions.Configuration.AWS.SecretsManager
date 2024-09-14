# BD.Extensions.Configuration.AWS.SecretsManager

This is an AWS Secrets Manager configuration provider implementation for Microsoft.Extensions.Configuration. This package enables you to read your application's settings from a secret in AWS Secret Manager. The secret must be configured as key-value pair store. If you want to use nested object as key then add the colon, for example "Data:SqlConnectionString"..

The extension uses the [JsonStreamConfigurationSource](https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.Json/src/JsonStreamConfigurationSource.cs) object to load the json response returned when retrieving the secret value from AWS Secrets Manager.

To use in your application, simply install this extension from the public nuget repo and add the following code:

```csharp
        var appConfigSecretName = "data-development";

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddSecretsManager(appConfigSecretName)
            .Build();
```

The extension uses the AWS region defined in your AWS credentials file (development), or the AWS region it has rights to in an deployed environment.