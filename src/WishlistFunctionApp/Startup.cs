using ItsyBIT.Utilities;
using ItsyBitseList.Core;
using ItsyBitseList.Infrastructure;
using ItsyBitseList.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

[assembly: FunctionsStartup(typeof(WishlistFunctionApp.Startup))]
namespace WishlistFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = BuildConfiguration(builder.GetContext().ApplicationRootPath);

            builder.Services.AddCoreDependencies();
            var key = configuration.GetSection("EncodedIdentifierKey").Value;
            builder.Services.AddEncodedIdentifierGenerator(key);
            //bind settings
            builder.Services.Configure<StorageSettings>(configuration.GetSection(nameof(StorageSettings)));
            builder.Services.AddStorage();
            builder.Services.AddScoped<HttpContextAccessor>();
            builder.Services.AddMvcCore().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        }
        private IConfiguration BuildConfiguration(string applicationRootPath)
        {
            var config =
                new ConfigurationBuilder()
                    .SetBasePath(applicationRootPath)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            return config;
        }
    }
}
