using Problem01.BulkExternalRequests.Api.Models.Dtos;

namespace Problem01.BulkExternalRequests.Api.Clients
{
    public interface IExternalUserClient
    {
        Task<ExternalUserDto> GetUserAsync(int userId);
    }
}
