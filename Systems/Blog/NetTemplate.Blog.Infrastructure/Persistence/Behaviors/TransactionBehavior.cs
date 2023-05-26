using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.Infrastructure.Persistence.Behaviors;

namespace NetTemplate.Blog.Infrastructure.Persistence.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>
        : BaseTransactionBehavior<TransactionBehavior<TRequest, TResponse>, TRequest, TResponse, MainDbContext>
        , IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public TransactionBehavior(
            MainDbContext dbContext,
            IUnitOfWork unitOfWork,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, unitOfWork, logger)
        {
        }
    }
}
