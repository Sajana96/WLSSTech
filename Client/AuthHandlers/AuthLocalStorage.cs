using Microsoft.JSInterop;

namespace Client.AuthHandlers;

public class AuthLocalStorage
{
    private readonly IJSRuntime _js;
    public AuthLocalStorage(IJSRuntime js) => _js = js;

    public ValueTask SetTokenAsync(string token)
        => _js.InvokeVoidAsync("authStorage.setToken", token);

    public ValueTask<string?> GetTokenAsync()
        => _js.InvokeAsync<string?>("authStorage.getToken");

    public ValueTask ClearTokenAsync()
        => _js.InvokeVoidAsync("authStorage.clearToken");
}
