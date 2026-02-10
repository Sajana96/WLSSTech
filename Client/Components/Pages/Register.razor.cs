using Common.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Client.Components.Pages
{
    public partial class Register
    {
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private RegisterModel _model = new();
        private bool _busy;
        private string? _error;
        private string? _success;

        private async Task HandleRegister()
        {
            _busy = true;
            _error = null;
            _success = null;

            try
            {
                var client = HttpClientFactory.CreateClient("Api"); // or "ApiAuth" if you prefer

                // matches your API: UserName = Name, Email = Email, Password = Password
                var resp = await client.PostAsJsonAsync("auth/register", new RegisterRequest(_model.Email, _model.Password, _model.Name));
               

                if (resp.IsSuccessStatusCode)
                {
                    _success = "Account created successfully. Redirecting to login...";
                    await Task.Delay(600);
                    Nav.NavigateTo("/");
                    return;
                }

                // Try to parse your ApiResponse or validation errors
                var body = await resp.Content.ReadAsStringAsync();

                if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    _error = "Registration failed. Please check your details. " + body;
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _error = "Unauthorized. (Register endpoint should be AllowAnonymous on API.)";
                }
                else
                {
                    _error = $"Registration failed ({(int)resp.StatusCode}). {body}";
                }
            }
            catch (Exception ex)
            {
                _error = $"Registration error: {ex.Message}";
            }
            finally
            {
                _busy = false;
            }
        }
            private sealed class RegisterModel
        {
            [Required, MinLength(2), MaxLength(50)]
            public string Name { get; set; } = "";

            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [Required, MinLength(8), MaxLength(100)]
            public string Password { get; set; } = "";
        }
    }
}
