using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Microsoft.Extensions.Options;
// using Spire.Xls;
// using Spire.Xls.Core.MergeSpreadsheet.Interfaces;

namespace DigitalSignatureApi
{
  public class DocSigner: IDocSigner
  {
    private AppConfig config;
    public DocSigner(IOptions<AppConfig> config) {
      this.config = config.Value;
      Console.WriteLine(config.Value.Certificate);
    }
    public SignResponse Sign(SignRequest request)
    {
      var certFile = this.config.Certificate;
      var password = this.config.Password;

      var filePfx = Path.Combine(Path.GetTempPath(), "default.pfx");
      File.WriteAllBytes(filePfx, Convert.FromBase64String(certFile));
      X509Certificate2 certificate = new X509Certificate2(filePfx, password);

      Aspose.Cells.Workbook workbook;

      using(var ms = new MemoryStream(Convert.FromBase64String(request.Document))) {
        workbook = new Aspose.Cells.Workbook(ms);
      }

      //Create the digital signature collection
      Aspose.Cells.DigitalSignatures.DigitalSignatureCollection dsCollection = new Aspose.Cells.DigitalSignatures.DigitalSignatureCollection();

      //Create new digital signature and add it in digital signature collection
      Aspose.Cells.DigitalSignatures.DigitalSignature signature = new Aspose.Cells.DigitalSignatures.DigitalSignature(certificate, "Aspose.Cells added new digital signature in existing digitally signed workbook.", DateTime.Now);
      dsCollection.Add(signature);

      //Add digital signature collection inside the workbook
      workbook.AddDigitalSignature(dsCollection);
        byte[] output;

      using (var ms = new MemoryStream())
      {
          workbook.Save(ms, Aspose.Cells.SaveFormat.Xlsx);
          ms.Seek(0, SeekOrigin.Begin);
          output = ms.ToArray();
      }
      
      return new SignResponse()
      {
        Time = DateTime.Now.ToString("s"),
        SignedDocument = Convert.ToBase64String(output, 0)
      };

      //try
      //{
      //   Workbook workbook = new Workbook();

      //   //Load an Excel file

      //   // workbook.LoadFromFile("Sample.xlsx");

      //   //Add digital signature to the file

      //   

      //   DateTime certtime = new DateTime(2020, 7, 1, 7, 10, 36);

      //   IDigitalSignatures signature = workbook.AddDigitalSignature(cert, "e-iceblue", certtime);



      //   //Save the result file
      //   byte[] output;

      //   using (var ms = new MemoryStream())
      //   {
      //     workbook.SaveToStream(ms);
      //     ms.Seek(0, SeekOrigin.Begin);
      //     output = ms.ToArray();
      //   }
      //   return Convert.ToBase64String(output, 0);
      // }
      // catch(Exception e) {
      //   return e.Message + e.StackTrace;
      // }
    }
  }

}
