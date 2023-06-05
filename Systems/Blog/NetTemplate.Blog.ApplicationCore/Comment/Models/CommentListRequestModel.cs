using NetTemplate.Shared.ApplicationCore.Common.Interfaces;
using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Comment.Models
{
    public class CommentListRequestModel : BasePagingQuery, IKeySetPagingQuery<DateTimeOffset>, IByCreatorQuery<int>
    {
        public int? CreatorId { get; set; }
        public DateTimeOffset? KeyAfter { get; set; }
        public DateTimeOffset? KeyBefore { get; set; }

        public override bool CanGetAll() => false;
    }
}
