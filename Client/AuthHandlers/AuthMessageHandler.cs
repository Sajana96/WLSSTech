using System.Net.Http.Headers;

namespace Client.AuthHandlers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly TokenStore _tokenStore;
            private readonly AuthLocalStorage _authLocalStorage;

        public AuthMessageHandler(TokenStore tokenStore, AuthLocalStorage authLocalStorage)
        {
            _tokenStore = tokenStore;
            _authLocalStorage = authLocalStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var token = await _authLocalStorage.GetTokenAsync();
            var token =  _tokenStore.Token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
