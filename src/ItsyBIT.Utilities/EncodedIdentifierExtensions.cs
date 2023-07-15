using Microsoft.Extensions.DependencyInjection;

namespace ItsyBIT.Utilities
{
    public static class EncodedIdentifierExtensions
    {
        public static IServiceCollection AddEncodedIdentifierGenerator(this IServiceCollection services, string key)
        {
            services.AddSingleton(new EncodedIdentifierGenerator(key));
            return services;
        }
    }
}
