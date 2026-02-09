using Client.DTO;
using Common.Contracts;
using Common.Enums;
using System.Net.Http.Json;

namespace Client.Services;

public class IncidentService
{
    private readonly IHttpClientFactory _factory;

    public IncidentService(IHttpClientFactory factory)
    {
        _factory = factory;
    }


    public async Task<GetAllIncidentResponse> GetAllAsync()
    {
        var Client = _factory.CreateClient("ApiAuth");
        var response = await Client.GetFromJsonAsync<GetAllIncidentResponse>("incident/getAll");
        return response ?? response ?? new GetAllIncidentResponse { };
    }

    public async Task CreateAsync(CreateIncidentRequest req)
    {
        var Client = _factory.CreateClient("ApiAuth");
        var resp = await Client.PostAsJsonAsync("incident/create", req);
        resp.EnsureSuccessStatusCode();
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var Client = _factory.CreateClient("ApiAuth");
        var resp = await Client.PutAsJsonAsync($"incident/{id}", new UpdateIncidentRequest() { Status= (IncidentStatus)Enum.Parse(typeof(IncidentStatus), status) });
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var Client = _factory.CreateClient("ApiAuth");

        var resp = await Client.DeleteAsync($"incident/{id}");
        resp.EnsureSuccessStatusCode();
    }
}
