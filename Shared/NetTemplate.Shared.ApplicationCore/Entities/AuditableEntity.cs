namespace NetTemplate.Shared.ApplicationCore.Entities
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
    }

    public interface IAuditableEntity<TUserKey> : IAuditableEntity
    {
        public TUserKey CreatorId { get; set; }
        public TUserKey LastModifyUserId { get; set; }
    }

    public interface ISoftDeleteEntity
    {
        public DateTimeOffset? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public interface ISoftDeleteEntity<TUserKey> : ISoftDeleteEntity
    {
        public TUserKey DeletorId { get; set; }
    }
}
