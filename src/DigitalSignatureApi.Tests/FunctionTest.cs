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

        var res = await client.PostAsync("https://dfh1v3m3zc.execute-api.ap-southeast-2.amazonaws.com/dev", content);
        var data = await res.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<SignResponse>(data);
        Assert.NotNull(result.SignedDocument);

        File.WriteAllBytes(Path.Combine("../../../TestOutputs", "signed.xlsx"), Convert.FromBase64String(result.SignedDocument));
      }
    }
  }

}
