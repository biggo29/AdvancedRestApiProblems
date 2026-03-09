using Problem01.BulkExternalRequests.Api.Models.Dtos;
using System.Text.Json;

namespace Problem01.BulkExternalRequests.Api.Clients
{
    public class ExternalUserClient : IExternalUserClient
    {
        private readonly HttpClient _httpClient;
        public ExternalUserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ExternalUserDto> GetUserAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"users/{userId}");
            
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ExternalUserDto>();
        }
    }
}
