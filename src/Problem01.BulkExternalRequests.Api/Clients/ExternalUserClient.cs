using Problem01.BulkExternalRequests.Api.Models.Dtos;
using System.Text.Json;

namespace Problem01.BulkExternalRequests.Api.Clients
{
    public class ExternalUserClient : IExternalUserClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalUserClient> _logger;
        public ExternalUserClient(
            HttpClient httpClient, 
            ILogger<ExternalUserClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ExternalUserDto> GetUserAsync(
            int userId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.GetAsync($"users/{userId}", cancellationToken);
                if(!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch user with ID {UserId}. Status Code: {StatusCode}", userId, response.StatusCode);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<ExternalUserDto>(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request for user with ID {UserId} was cancelled.", userId);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID {UserId}.", userId);
                return null;
            }
             //var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
             //return await JsonSerializer.DeserializeAsync<ExternalUserDto>(contentStream, cancellationToken: cancellationToken);
            //var response = await _httpClient.GetAsync($"users/{userId}");

            //if(!response.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            //return await response.Content.ReadFromJsonAsync<ExternalUserDto>();
        }
    }
}
