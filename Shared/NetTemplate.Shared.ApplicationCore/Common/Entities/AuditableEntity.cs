namespace NetTemplate.Shared.ApplicationCore.Common.Entities
{
    public interface IAuditableEntity
    {
        DateTimeOffset CreatedTime { get; }
        DateTimeOffset? LastModifiedTime { get; }

        void UpdateCreatedTime();
        void UpdateLastModifiedTime();
    }

    public interface IAuditableEntity<TUserKey> : IAuditableEntity where TUserKey : struct
    {
        TUserKey? CreatorId { get; }
        TUserKey? LastModifyUserId { get; }

        void SetCreatorId(TUserKey? key);
        void SetLastModifyUserId(TUserKey? key);
    }

    public interface ISoftDeleteEntity
    {
        DateTimeOffset? DeletedTime { get; }
        bool IsDeleted { get; }

        void SoftDelete();
    }

    public interface ISoftDeleteEntity<TUserKey> : ISoftDeleteEntity where TUserKey : struct
    {
        TUserKey? DeletorId { get; }

        void SetDeletorId(TUserKey? key);
    }
}
