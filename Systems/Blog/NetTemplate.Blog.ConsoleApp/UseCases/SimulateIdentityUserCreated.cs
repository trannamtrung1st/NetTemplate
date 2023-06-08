using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetTemplate.ApacheKafka.Utils;
using NetTemplate.Shared.ApplicationCore.Domains.Identity.Models;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Extensions;
using NetTemplate.Shared.Infrastructure.PubSub.ApacheKafka.Models;
using TopicNames = NetTemplate.Blog.Infrastructure.Integrations.Identity.Constants.TopicNames;

namespace NetTemplate.Blog.ConsoleApp.UseCases
{
    public static class SimulateIdentityUserCreated
    {
        public static async Task Run(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            using IServiceScope scope = provider.CreateScope();
            IConfiguration configuration = scope.ServiceProvider.GetService<IConfiguration>();
            IOptions<ApacheKafkaConfig> kafkaOptions = scope.ServiceProvider.GetService<IOptions<ApacheKafkaConfig>>();

            IdentityUserModel model = InputModel();

            CloneableProducerConfig producerConfig = GetConfig(configuration, kafkaOptions.Value.CommonProducerConfig);

            using IProducer<string, IdentityUserCreatedEventModel> producer = KafkaHelper.CreateProducer<string, IdentityUserCreatedEventModel>(producerConfig);

            await Produce(producer, model);
        }

        static async Task Produce(IProducer<string, IdentityUserCreatedEventModel> producer, IdentityUserModel model)
        {
            await producer.ProduceAsync(TopicNames.IdentityUserCreated, new Message<string, IdentityUserCreatedEventModel>
            {
                Key = model.UserCode,
                Value = new IdentityUserCreatedEventModel
                {
                    Model = model
                }
            });
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

        static CloneableProducerConfig GetConfig(IConfiguration configuration, CloneableProducerConfig commonConfig)
        {
            IConfigurationSection section = configuration.GetProducerConfig("SimulateIdentityUserCreated");

            CloneableProducerConfig producerConfig = (CloneableProducerConfig)commonConfig.Clone();

            section.Bind(producerConfig);

            return producerConfig;
        }
    }
}
