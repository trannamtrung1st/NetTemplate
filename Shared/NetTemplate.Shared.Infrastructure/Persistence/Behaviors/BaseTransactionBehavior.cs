using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Reflection;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;

namespace NetTemplate.Shared.Infrastructure.Persistence.Behaviors
{
    public abstract class BaseTransactionBehavior<T, TRequest, TResponse, TDbContext> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TDbContext : DbContext
    {
        private readonly ILogger<T> _logger;
        private readonly TDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BaseTransactionBehavior(
            TDbContext dbContext,
            IUnitOfWork unitOfWork,
            ILogger<T> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(null, nameof(DbContext));
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentException(null, nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IAutoSaveCommand == false)
            {
                return await next();
            }

            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            if (request is IBaseTransactionalCommand)
            {
                try
                {
                    if (_dbContext.HasActiveTransaction())
                    {
                        return await next();
                    }

                    var strategy = _dbContext.Database.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = await _dbContext.BeginTransactionOrCurrent())
                        {
                            try
                            {
                                _logger.LogInformation("[START] Begin transaction {transactionId} for {commandName}", transaction.TransactionId, typeName);

                                response = await next();

                                await _unitOfWork.CommitChanges();

                                _logger.LogInformation("[END] Commit transaction {transactionId} for {commandName}", transaction.TransactionId, typeName);

                                await transaction.CommitAsync();
                            }
                            catch (Exception)
                            {
                                await transaction.RollbackAsync();

                                throw;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ERROR] Handling transaction for {commandName} ({@command})", typeName, request);
                    throw;
                }
            }
            else
            {
                response = await next();

                await _unitOfWork.CommitChanges();
            }

            return response;
        }
    }
}
