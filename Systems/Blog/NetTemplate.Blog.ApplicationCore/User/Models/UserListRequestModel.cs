using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.User.Models
{
    public class UserListRequestModel : BaseSortableFilterQuery<Enums.UserSortBy>
    {
        public IEnumerable<string> UserCodes { get; set; }
        public IEnumerable<int> Ids { get; set; }
        public bool? Active { get; set; }

        public override bool CanGetAll() => false;
    }
}
