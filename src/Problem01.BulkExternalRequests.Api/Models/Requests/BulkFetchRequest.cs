namespace Problem01.BulkExternalRequests.Api.Models.Requests
{
    public class BulkFetchRequest
    {
        public List<int> UserIds { get; set; } = new List<int>();
    }
}
