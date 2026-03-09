using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Problem01.BulkExternalRequests.Api.Models.Requests;
using Problem01.BulkExternalRequests.Api.Models.Responses;
using Problem01.BulkExternalRequests.Api.Services;

namespace Problem01.BulkExternalRequests.Api.Controllers
{
    [Route("api/bulk")]
    [ApiController]
    public class BulkFetchController : ControllerBase
    {
        private readonly IBulkFetchService _bulkFetchService;

        public BulkFetchController(IBulkFetchService bulkFetchService)
        {
            _bulkFetchService = bulkFetchService;
        }

        [HttpPost("users")]
        public async Task<ActionResult<BulkFetchResponse>> FetchUsers(
            [FromBody] BulkFetchRequest request,
            CancellationToken cancellationToken)
        {

            //var users = await _bulkFetchService.FetchUsersAsync(request.UserIds);

            //return Ok(new BulkFetchResponse
            //{
            //    Users = users
            //});

            if (request.UserIds is null || request.UserIds.Count == 0)
            {
                return BadRequest("UserIds cannot be null or empty.");
            }

            var result = await _bulkFetchService.FetchUsersAsync(request.UserIds, cancellationToken);

            return Ok(result);
        }
    }
}
