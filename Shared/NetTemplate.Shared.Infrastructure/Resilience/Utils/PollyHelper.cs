using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;
using Polly.Retry;
using System.Net;

namespace NetTemplate.Shared.Infrastructure.Resilience.Utils
{
    public static class PollyHelper
    {
        public const string TransientErrorPolicy = nameof(TransientErrorPolicy);
        public const string TransientHttpErrorPolicy = nameof(TransientHttpErrorPolicy);

        // High throughput scenario: https://github.com/App-vNext/Polly/wiki/Avoiding-cache-repopulation-request-storms
        public static PolicyRegistry InitPolly(IServiceProvider serviceProvider)
        {
            PolicyRegistry registry = new PolicyRegistry
            {
                { TransientErrorPolicy, BuildCommonTransientErrorPolicy() },
                { TransientHttpErrorPolicy, BuildCommonTransientHttpErrorPolicy() }
            };

            return registry;
        }

        private static IAsyncPolicy BuildCommonTransientErrorPolicy()
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2), retryCount: 3);

            AsyncRetryPolicy commonRetry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(jitterDelay, onRetry: (exception, delay, count, context) => { });

            return commonRetry;
        }

        private static IAsyncPolicy<HttpResponseMessage> BuildCommonTransientHttpErrorPolicy()
        {
            var jitterDelay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2), retryCount: 3);

            var httpRetry = Policy<HttpResponseMessage>
                .HandleResult(resp => (int)resp.StatusCode >= (int)HttpStatusCode.InternalServerError)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(jitterDelay);

            return httpRetry;
        }
    }
}
