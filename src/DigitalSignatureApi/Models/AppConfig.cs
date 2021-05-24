using System.Text.Json.Serialization;

namespace DigitalSignatureApi
{
  public class CertificateConfig
  {
    public string Certificate { get; set; }

    public string Password { get; set; }
  }

  public class AppConfig {
    public CertificateConfig Certificate { get; set; }
  }
}

