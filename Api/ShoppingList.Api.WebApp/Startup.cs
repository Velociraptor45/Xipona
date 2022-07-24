using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Core.Files;
using ProjectHermes.ShoppingList.Api.Domain;
using ProjectHermes.ShoppingList.Api.Endpoint;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using ProjectHermes.ShoppingList.Api.Vault;
using ProjectHermes.ShoppingList.Api.WebApp.Services;
using Serilog;

namespace ProjectHermes.ShoppingList.Api.WebApp;

public class Startup
{
    private readonly IWebHostEnvironment _environment;
    private const int _mebibyte = 1024 * 1024;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _environment = env;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(GetFileLoggingPath(), rollOnFileSizeLimit: true, fileSizeLimitBytes: _mebibyte)
            .CreateLogger();

        var fileLoadingService = new FileLoadingService();
        var vaultService = new VaultService(Configuration, fileLoadingService);
        var configurationLoadingService = new ConfigurationLoadingService(fileLoadingService, vaultService);
        var connectionStrings = configurationLoadingService.LoadAsync().GetAwaiter().GetResult();
        services.AddSingleton(connectionStrings);

        services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        services.AddDomain();
        services.AddEndpointControllers();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.ToString());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("temporary",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); //todo
                });
        });
        services.AddInfrastructure();
        services.AddApplicationServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingList API v1");
        });

        app.UseRouting();
        app.UseCors("temporary");
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private string GetFileLoggingPath()
    {
        if (_environment.IsEnvironment("Local"))
            return "./logs/logs.txt";

        return $"/app/logs/{_environment.EnvironmentName}-ph-sl-api-logs.txt";
    }
}