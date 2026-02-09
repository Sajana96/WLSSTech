using Microsoft.AspNetCore.Components;

namespace Client.AuthHandlers
{
    public abstract class AuthGuard : ComponentBase
    {
        [Inject] protected TokenStore TokenStore { get; set; } = default!;
        [Inject] protected NavigationManager Nav { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (!TokenStore.IsAuthenticated)
            {
                Nav.NavigateTo("/");
            }
        }
    }
}
