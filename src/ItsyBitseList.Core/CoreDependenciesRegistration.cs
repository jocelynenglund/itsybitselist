using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ItsyBitseList.Core
{
    /// <summary>
    /// This class contains the registration of all dependencies for the Core project
    /// </summary>
    public static class CoreDependenciesRegistration
    {

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            return services;
        }
    }
}
