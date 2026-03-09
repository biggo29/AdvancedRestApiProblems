using Problem01.BulkExternalRequests.Api.Models.Dtos;

namespace Problem01.BulkExternalRequests.Api.Models.Responses
{
    public class BulkFetchResponse
    {
        public List<ExternalUserDto> Users { get; set; } = new();

        public int TotalFetched => Users.Count;
    }
}
