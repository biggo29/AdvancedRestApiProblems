using Problem01.BulkExternalRequests.Api.Clients;
using Problem01.BulkExternalRequests.Api.Services;

namespace Problem01.BulkExternalRequests.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBulkFetchService, BulkFetchService>();
            services.AddHttpClient<IExternalUserClient, ExternalUserClient>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            });
            return services;
        }
    }
}
