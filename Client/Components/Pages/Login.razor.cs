using Client.AuthHandlers;
using Common.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Client.Components.Pages;

public partial class Login
{
    [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] private AuthLocalStorage LocalStorage { get; set; } = default!;
    [Inject] private TokenStore TokenStore { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    protected LoginModel _model = new();
    protected bool _busy;
    protected string? _error;

    public async Task HandleLogin()
    {
        _busy = true;
        _error = null;

        try
        {
            var client = HttpClientFactory.CreateClient("Api");

            var resp = await client.PostAsJsonAsync("auth/login", new LoginRequest(_model.Email, _model.Password));                 

            if (!resp.IsSuccessStatusCode)
            {
                _error = "Invalid email or password.";
                return;
            }

            var json = await resp.Content.ReadFromJsonAsync<LoginResponse>();
            if (json?.Token is null)
            {
                _error = "Login failed: no token returned.";
                return;
            }

            TokenStore.Set(json.Token);
            await LocalStorage.SetTokenAsync(json.Token);
            Nav.NavigateTo("/incidents");
        }
        catch (Exception ex)
        {
            _error = $"Login error: {ex.Message}";
        }
        finally
        {
            _busy = false;
        }
    }

    protected sealed class LoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }

    protected sealed class LoginResponse
    {
        public string Token { get; set; } = "";
    }
}
