using Problem01.BulkExternalRequests.Api.Models.Dtos;

namespace Problem01.BulkExternalRequests.Api.Models.Responses
{
    public class BulkFetchResponse
    {
        public List<ExternalUserDto> Users { get; set; } = new();
        public List<int> FailedUserIds { get; set; } = new();
        public int TotalRequested {  get; set; }

        public int TotalFetched => Users.Count;
        public int TotalFailed => FailedUserIds.Count;
    }
}
