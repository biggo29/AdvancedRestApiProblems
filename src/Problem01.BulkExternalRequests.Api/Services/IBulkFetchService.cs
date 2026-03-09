using Problem01.BulkExternalRequests.Api.Models.Dtos;
using Problem01.BulkExternalRequests.Api.Models.Responses;

namespace Problem01.BulkExternalRequests.Api.Services
{
    public interface IBulkFetchService
    {
        Task<BulkFetchResponse> FetchUsersAsync(List<int> userIds, CancellationToken cancellationToken = default);
    }
}
