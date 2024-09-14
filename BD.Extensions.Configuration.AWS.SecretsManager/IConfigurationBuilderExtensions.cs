using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration.Json;
using System.Text;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Based on: https://aws.amazon.com/blogs/modernizing-with-aws/how-to-load-net-configuration-from-aws-secrets-manager/
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads json configuration from a secret in AWS Secrets Manager.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="secretName">The name of the secret to read keys from.</param>
    public static IConfigurationBuilder AddSecretsManager(
        this IConfigurationBuilder builder,
        string secretName) =>
        builder.Add<JsonStreamConfigurationSource>(s => s.Stream = new MemoryStream(Encoding.Default.GetBytes(GetSecretKeysAsJson(secretName))));

    #region Private
    private static string GetSecretKeysAsJson(string secretName)
    {
        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName
        };

        using (var client = new AmazonSecretsManagerClient())
        {
            var response = Task.Run(() => client.GetSecretValueAsync(request)).GetAwaiter().GetResult();

            string secretString;
            if (response.SecretString != null)
            {
                secretString = response.SecretString;
            }
            else
            {
                var memoryStream = response.SecretBinary;
                var reader = new StreamReader(memoryStream);
                secretString = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            return secretString; // Key-value pairs are returned as plaintext json
        }
    }
    #endregion Private
}