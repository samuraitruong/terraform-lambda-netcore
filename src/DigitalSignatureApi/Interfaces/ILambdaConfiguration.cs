using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace DigitalSignatureApi
{
  public interface ILambdaConfiguration
  {
       IConfigurationRoot Configuration { get; }

  }

}

