using Problem01.BulkExternalRequests.Api.Clients;
using Problem01.BulkExternalRequests.Api.Models.Dtos;
using Problem01.BulkExternalRequests.Api.Models.Responses;
using System.Collections.Concurrent;

namespace Problem01.BulkExternalRequests.Api.Services
{
    public class BulkFetchService : IBulkFetchService
    {
        private readonly IExternalUserClient _externalUserClient;
        private readonly ILogger _logger;
        public BulkFetchService(IExternalUserClient externalUserClient, ILogger logger)
        {
            _externalUserClient = externalUserClient;
            _logger = logger;
        }

        public async Task<BulkFetchResponse> FetchUsersAsync(List<int> userIds, CancellationToken cancellationToken = default)
        {
            //var tasks = userIds.Select(id => _externalUserClient.GetUserAsync(id)).ToList();
            //var ressults = await Task.WhenAll(tasks);
            //return ressults
            //    .Where(x => x != null)
            //    .Select(x => x!)
            //    .ToList();
            var users = new ConcurrentBag<ExternalUserDto>();
            var failedUserIds = new ConcurrentBag<int>();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(userIds, options, async (userId, ct) =>
            {
                try
                {
                    var user = await _externalUserClient.GetUserAsync(userId, ct);
                    if (user is not null)
                    {
                        users.Add(user);
                    }
                    else
                    {
                        failedUserIds.Add(userId);
                        _logger.LogWarning("User with ID {UserId} not found.", userId);
                    }

                    _logger.LogInformation("Successfully fetched user with ID {UserId}.", userId);

                }
                catch (Exception ex)
                {
                    failedUserIds.Add(userId);
                    _logger.LogError(ex, "Error fetching user with ID {UserId}.", userId);
                }
            });

            return new BulkFetchResponse
            {
                TotalRequested = userIds.Count,
                Users = users.OrderBy(u => u.Id).ToList(),
                FailedUserIds = failedUserIds.OrderBy(id => id).ToList()
            };
        }
    }
}
