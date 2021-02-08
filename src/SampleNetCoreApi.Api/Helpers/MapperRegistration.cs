using SampleNetCoreApi.Api.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace SampleNetCoreApi.Api.Helpers
{
    public static class MapperRegistration
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile));

            return services;
        }
    }
}
