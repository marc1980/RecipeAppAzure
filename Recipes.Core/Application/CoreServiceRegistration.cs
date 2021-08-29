using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Recipes.Core.Application
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices( this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
