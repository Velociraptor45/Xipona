using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProjectHermes.ShoppingList.Api.WebApp.Services;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace ProjectHermes.ShoppingList.Api.WebApp;

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
                    IPAddress ipAddress = IPAddress.Parse(configuration["Kestrel:IP-Address"]);
                    int port = int.Parse(configuration["Kestrel:Port"]);
                    options.Listen(ipAddress, port, listenOptions =>
                    {
                        listenOptions.UseHttps(GetCertificate(configuration));
                    });
                });
                webBuilder.UseStartup<Startup>();
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            });
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true, reloadOnChange: true)
            .Build();
    }

    private static X509Certificate2 GetCertificate(IConfiguration configuration)
    {
        CertificateLoadingService loadingService = new CertificateLoadingService();
        string crtFilePath = configuration["Certificate:CrtFilePath"];
        string privateKeyFilePath = configuration["Certificate:PrivateKeyFilePath"];

        return loadingService.GetCertificate(crtFilePath, privateKeyFilePath);
    }
}