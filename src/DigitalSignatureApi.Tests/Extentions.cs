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
  public static class Helper{
    public static string ReadAsBase64(string filename){
      return Convert.ToBase64String(File.ReadAllBytes(filename));
    }
  }
}
