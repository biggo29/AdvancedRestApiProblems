using Problem01.BulkExternalRequests.Api.Clients;
using Problem01.BulkExternalRequests.Api.Models.Dtos;

namespace Problem01.BulkExternalRequests.Api.Services
{
    public class BulkFetchService : IBulkFetchService
    {
        private readonly IExternalUserClient _externalUserClient;
        public BulkFetchService(IExternalUserClient externalUserClient)
        {
            _externalUserClient = externalUserClient;
        }

        public async Task<List<ExternalUserDto>> FetchUsersAsync(List<int> userIds)
        {
            var tasks = userIds.Select(id => _externalUserClient.GetUserAsync(id)).ToList();
            var ressults = await Task.WhenAll(tasks);
            return ressults
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
        }
    }
}
