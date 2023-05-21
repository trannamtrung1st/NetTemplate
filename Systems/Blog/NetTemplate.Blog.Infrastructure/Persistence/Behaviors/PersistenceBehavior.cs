using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTemplate.Common.Reflection;
using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.Infrastructure.Persistence.Behaviors
{
    public class PersistenceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PersistenceBehavior<TRequest, TResponse>> _logger;
        private readonly MainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public PersistenceBehavior(
            MainDbContext dbContext,
            IUnitOfWork unitOfWork,
            ILogger<PersistenceBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(MainDbContext));
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
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
                    if (_dbContext.HasActiveTransaction)
                    {
                        return await next();
                    }

                    var strategy = _dbContext.Database.CreateExecutionStrategy();

                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = await _dbContext.BeginTransactionAsync())
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
