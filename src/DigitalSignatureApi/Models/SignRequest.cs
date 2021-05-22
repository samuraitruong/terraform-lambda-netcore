using System.Text.Json.Serialization;

namespace DigitalSignatureApi
{
  public class SignCertificate{
    [JsonPropertyName("certificate")]
    public string Certificate { get; set; }
[JsonPropertyName("password")]
    public string Password { get; set; }
  }
  public class SignRequest
  {
    [JsonPropertyName("document")]
    public string Document { get; set; }

    [JsonPropertyName("s3InputUrl")]
    public string S3InputUrl { get; set; }

    [JsonPropertyName("s3OutputUrl")]
    public string S3OutputUrl { get; set; }


[JsonPropertyName("certificate")]
  public SignCertificate Certificate { get; set; }

  }
}

