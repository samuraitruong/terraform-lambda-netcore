using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using DigitalSignatureApi;
using System.Net.Http;
using System.Text;
using System.IO;

namespace DigitalSignatureApi.Tests
{
  public class FunctionTest
  {
    [Fact]
    public void TestToUpperFunction()
    {

      // Invoke the lambda function and confirm the string was upper cased.
      var function = new Function();
      var context = new TestLambdaContext();
      var upperCase = function.FunctionHandler("hello world", context);

      Assert.Equal("HELLO WORLD", upperCase);
    }

    [Fact]
    public async void LiveFunctionTest()
    {

      using (var client = new HttpClient())
      {
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new SignRequest()
        {
          Document = Helper.ReadAsBase64("../../../TestInputs/sample.xlsx")
        }),
        Encoding.UTF8,
        "application/json");

        var message = new HttpRequestMessage()
        {
          Method = HttpMethod.Post,
          RequestUri = new Uri("https://djtvp791sh.execute-api.ap-southeast-2.amazonaws.com/dev"),
          Content = content,
          Headers = {
            { "x-api-key", "53c1136e-81a9-4dd1-a6f9-6759ea53ad9a"}
         }
        };

        var res = await client.SendAsync(message);
        var data = await res.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<SignResponse>(data);
        Assert.NotNull(result.SignedDocument);

        File.WriteAllBytes(Path.Combine("../../../TestOutputs", "signed.xlsx"), Convert.FromBase64String(result.SignedDocument));
      }
    }
  }

}
