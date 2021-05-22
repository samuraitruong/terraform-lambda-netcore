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
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
// [assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

[assembly: Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

//https://aws.amazon.com/blogs/compute/developing-net-core-aws-lambda-functions/
namespace DigitalSignatureApi
{
    public class Function
    {

        private readonly IServiceProvider _serviceProvider;
        // https://garywoodfine.com/serverless-aws-lambda-dependency-injection/
        private ILambdaConfiguration Configuration { get; }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILambdaConfiguration, LambdaConfiguration>();
        }

        public Function() : this(StartUp.Container.BuildServiceProvider())
        {
        }

        public Function(IServiceProvider serviceProvider) {
         this._serviceProvider = serviceProvider;
        }
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
         return input.ToUpper();
        }


        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
                var signRequest = System.Text.Json.JsonSerializer.Deserialize<SignRequest>(request.Body);

                var docSigner = _serviceProvider.GetService<IDocSigner>();
                var output = docSigner.Sign(signRequest);
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = System.Text.Json.JsonSerializer.Serialize(output),
                    IsBase64Encoded =  false,
                    Headers = new Dictionary<string, string> { 
                        {"Content-Type", "application/json"},
                        {"Access-Control-Allow-Origin", "*"},
                        {"Access-Control-Allow-Credentials", "true"}
                     }
                };

            return response;
        }

    }
}
