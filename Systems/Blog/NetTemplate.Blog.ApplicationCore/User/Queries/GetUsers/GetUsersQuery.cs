using MediatR;
using NetTemplate.Blog.ApplicationCore.User.Models;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<ListResponseModel<UserListItemModel>>
    {
        public UserListRequestModel Model { get; }

        public GetUsersQuery(UserListRequestModel model)
        {
            Model = model;
        }
    }
}
