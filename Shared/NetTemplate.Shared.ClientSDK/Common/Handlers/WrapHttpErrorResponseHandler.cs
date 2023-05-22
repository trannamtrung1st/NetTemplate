using NetTemplate.Shared.ClientSDK.Common.Exceptions;

namespace NetTemplate.Shared.ClientSDK.Common.Handlers
{
    public class WrapHttpErrorResponseHandler : DelegatingHandler
    {
        public WrapHttpErrorResponseHandler()
        {
        }

        public WrapHttpErrorResponseHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await base.SendAsync(request, cancellationToken);

                response = response.EnsureSuccessStatusCode();

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpErrorResponseException(response, ex);
            }
        }
    }
}
