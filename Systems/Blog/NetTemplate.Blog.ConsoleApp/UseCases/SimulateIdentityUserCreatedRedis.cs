using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using TopicNames = NetTemplate.Blog.Infrastructure.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.ConsoleApp.UseCases
{
    public static class SimulateIdentityUserCreatedRedis
    {
        public static async Task Run(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            using IServiceScope scope = provider.CreateScope();
            ConnectionMultiplexer connectionMultiplexer = scope.ServiceProvider.GetRequiredService<ConnectionMultiplexer>();

            IdentityUserModel model = InputModel();

            await Produce(connectionMultiplexer, model);
        }

        static async Task Produce(ConnectionMultiplexer connectionMultiplexer, IdentityUserModel model)
        {
            await connectionMultiplexer.GetSubscriber().PublishAsync(TopicNames.IdentityUserCreated,
                JsonConvert.SerializeObject(new IdentityUserCreatedEventModel
                {
                    Model = model
                }));
        }

        static IdentityUserModel InputModel()
        {
            Console.Write("Input user code: ");
            string userCode = Console.ReadLine();

            Console.Write("Input first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Input last name: ");
            string lastName = Console.ReadLine();

            return new IdentityUserModel
            {
                UserCode = userCode,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true
            };
        }
    }
}
