using MediatR;

namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IBaseTransactionalCommand : IAutoSaveCommand { }

    public interface ITransactionalCommand : IBaseTransactionalCommand, IRequest { }

    public interface ITransactionalCommand<T> : IBaseTransactionalCommand, IRequest<T> { }
}
