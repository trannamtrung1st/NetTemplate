﻿namespace NetTemplate.Blog.ApplicationCore.Post.Models
{
    public abstract class BasePostResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorUserCode { get; set; }
        public string CreatorFullName { get; set; }
    }
}
