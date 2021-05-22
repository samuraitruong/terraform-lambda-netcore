using System.IO;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace DigitalSignatureApi
{
  public class LambdaConfiguration : ILambdaConfiguration
  {
        public static IConfigurationRoot Configuration  => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

         IConfigurationRoot ILambdaConfiguration.Configuration => Configuration;
         
  }
}


