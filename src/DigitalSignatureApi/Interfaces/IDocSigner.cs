using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace DigitalSignatureApi
{
  public interface IDocSigner
  {
    SignResponse Sign(SignRequest request);

  }

}

