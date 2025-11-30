using System.Text.Json;
using Application.DTOs.Users;
using Infrastructure.Persistence;

namespace Infrastructure.Common;

public class RetrieveExternalUsers :  IRetrieveExternalUsers
{
    private const string api = "https://jsonplaceholder.typicode.com/users";
    private readonly HttpClient _httpClient;

    public RetrieveExternalUsers(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<UserDto>> RetrieveExternalUsersAsync()
    {
        var response = await _httpClient.GetAsync(api);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var externalUsers = JsonSerializer.Deserialize<List<UserDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return externalUsers;
    }
}