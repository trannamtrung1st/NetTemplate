using Microsoft.Extensions.DependencyInjection;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Blog.ConsoleApp.UseCases
{
    public static class InsertLargeData
    {
        public static async Task Run(IServiceProvider provider, CancellationToken cancellationToken = default)
        {
            using IServiceScope scope = provider.CreateScope();
            IPostRepository postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
            IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            for (int i = 1; i <= 100000; i++)
            {
                PostEntity entity = new PostEntity(
                    title: $"Post {i}",
                    content: $"Content post {i}",
                    categoryId: 9,
                    new[] { new PostTagEntity($"Hello {i}") });

                if (i % 100 == 0)
                {
                    Console.WriteLine($"Current: post {i}");
                }

                await postRepository.Create(entity, cancellationToken);
            }

            await unitOfWork.CommitChanges(cancellationToken: cancellationToken);
        }
    }
}
