using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using DigitalSignatureApi;
using System.IO;

namespace DigitalSignatureApi.Tests
{
    public class DocSignerTest
    {
        [Fact]
        public void TestDocSign()
        {
            var function = new DocSigner();
              var result = function.Sign(new SignRequest()
              {
                Document = Helper.ReadAsBase64("../../../TestInputs/sample.xlsx")
            });
             Assert.Equal(true, result.Time.Length > 0);

              File.WriteAllBytes(Path.Combine("../../../TestOutputs", "signed_local.xlsx"), Convert.FromBase64String(result.SignedDocument));
        }
    }
}
