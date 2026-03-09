using Problem01.BulkExternalRequests.Api.Models.Dtos;

namespace Problem01.BulkExternalRequests.Api.Services
{
    public interface IBulkFetchService
    {
        Task<List<ExternalUserDto>> FetchUsersAsync(List<int> userIds);
    }
}
