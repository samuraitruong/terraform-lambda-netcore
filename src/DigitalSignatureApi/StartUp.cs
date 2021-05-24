using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalSignatureApi
{
  public class StartUp
  {
    public static IServiceCollection Container => ConfigureServices(LambdaConfiguration.Configuration);
        private static IServiceCollection ConfigureServices(IConfigurationRoot root)
        {
            var services = new ServiceCollection();
            
            // Wire up all your dependencies here
            services.Configure<CertificateConfig>(options => root.GetSection("certificate").Bind(options));
            
            services.AddTransient<IDocSigner, DocSigner>();
            
            return services;
        }
  }
}
