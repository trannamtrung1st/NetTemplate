﻿using NetTemplate.Shared.ApplicationCore.Common.Models;

namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public class PostListRequestModel : BaseSortableFilterQuery<ApplicationEnums.SortBy>
    {
        public int? CategoryId { get; set; }

        public override bool CanGetAll() => false;
    }
}
