using ItsyBIT.Utilities;
using ItsyBitseList.Core;
using ItsyBitseList.Core.Interfaces.App;
using ItsyBitseList.Infrastructure;
using ItsyBitseList.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItsyBitseList.App
{
    public static class ExtensionMethods
    {
        public static IActionResult AsActionResult<T>(this Response<T> response) {
            if (response.Status == Status.NotFound)
                return new NotFoundObjectResult(response.ErrorMessage);
            else if (response.Status == Status.Error)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            else
            {
                if (response.Result is Unit)
                    return new NoContentResult();
                else
                    return new OkObjectResult(response.Result);
            }
        }


        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreDependencies();
            services.AddEncodedIdentifierGenerator(configuration[EncodedIdentifierGenerator.EncoderKey]);
            services.AddStorage(configuration.GetSection(nameof(StorageSettings)));
            services.AddTransient<IWishlistApp, WishlistApp>();

            return services;
        }
    }
}
