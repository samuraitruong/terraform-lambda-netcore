using System.Text.Json.Serialization;

namespace DigitalSignatureApi
{
  public class AppConfig
  {
    public string Certificate { get; set; }

    public string Password { get; set; }
  }
}

