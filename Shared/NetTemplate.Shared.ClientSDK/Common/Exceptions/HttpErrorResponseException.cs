namespace NetTemplate.Shared.ClientSDK.Common.Exceptions
{
    public class HttpErrorResponseException : Exception
    {
        public HttpResponseMessage Response { get; }
        public Exception Exception { get; }

        public HttpErrorResponseException(HttpResponseMessage response, Exception ex)
        {
            Response = response;
            Exception = ex;
        }
    }
}
