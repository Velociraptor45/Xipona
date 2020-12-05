using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace ProjectHermes.ShoppingList.Api.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = GetConfiguration();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                        {
                            options.AddServerHeader = false;
                            options.Listen(IPAddress.Any, 443, listenOptions =>
                            {
                                listenOptions.UseHttps(GetCertificate(configuration));
                            });
                        });
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseUrls("https://localhost:443");
                });
        }

        public static X509Certificate2 GetCertificate(IConfiguration config)
        {
            var certificateSettings = config.GetSection("certificateSettings");
            string fileName = certificateSettings.GetValue<string>("filename");
            string password = certificateSettings.GetValue<string>("password");

            return new X509Certificate2(fileName, password);
        }

        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("certificate.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"certificate.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", 
                optional: true, reloadOnChange: true)
            .Build();
        }
    }
}