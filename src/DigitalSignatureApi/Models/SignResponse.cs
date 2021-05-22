using System.Text.Json.Serialization;

namespace DigitalSignatureApi
{
  public class SignResponse
  {
    [JsonPropertyName("time")]
    public string Time { get; set; }

[JsonPropertyName("signedDocument")]
    public string SignedDocument { get; set; }
  }
}

