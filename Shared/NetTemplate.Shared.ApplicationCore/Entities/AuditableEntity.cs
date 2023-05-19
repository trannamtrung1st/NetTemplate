namespace NetTemplate.Shared.ApplicationCore.Entities
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
    }

    public interface IAuditableEntity<TUserKey> : IAuditableEntity where TUserKey : struct
    {
        public TUserKey? CreatorId { get; set; }
        public TUserKey? LastModifyUserId { get; set; }
    }

    public interface ISoftDeleteEntity
    {
        public DateTimeOffset? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public interface ISoftDeleteEntity<TUserKey> : ISoftDeleteEntity where TUserKey : struct
    {
        public TUserKey? DeletorId { get; set; }
    }
}
