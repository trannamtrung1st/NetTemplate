using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.User.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ListResponseModel<UserListItemModel>>
    {
        private readonly IValidator<GetUsersQuery> _validator;
        private readonly IUserPartialRepository _userPartialRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        public GetUsersQueryHandler(
            IValidator<GetUsersQuery> validator,
            IUserPartialRepository userPartialRepository,
            IMapper mapper,
            ILogger<GetUsersQueryHandler> logger)
        {
            _validator = validator;
            _userPartialRepository = userPartialRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ListResponseModel<UserListItemModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            UserListRequestModel model = request.Model;

            QueryResponseModel<UserListItemModel> response = await _userPartialRepository.Query<UserListItemModel>(
                terms: model.Terms,
                ids: model.Ids,
                userCodes: model.UserCodes,
                active: model.Active,
                sortBy: model.SortBy,
                isDesc: model.IsDesc,
                paging: model,
                count: true);

            UserListItemModel[] list = response.Query.ToArray();

            return new ListResponseModel<UserListItemModel>(response.Total.Value, list);
        }
    }
}
