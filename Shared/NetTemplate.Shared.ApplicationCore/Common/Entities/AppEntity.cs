using NetTemplate.Common.Reflection;

namespace NetTemplate.Shared.ApplicationCore.Common.Entities
{
    public abstract class AppEntity : DomainEntity
    {
    }

    public abstract class AppEntity<TId> : AppEntity, IHasId<TId>
    {
        public virtual TId TransientIdValue() => default;

        public bool IsTransient() => Equals(Id, TransientIdValue());

        private int? _requestedHashCode;
        private TId _id;

        protected AppEntity() : base()
        {
        }

        public virtual TId Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AppEntity<TId>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            AppEntity<TId> item = (AppEntity<TId>)obj;

            return item.Id?.Equals(Id) == true;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        public static bool operator ==(AppEntity<TId> left, AppEntity<TId> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(AppEntity<TId> left, AppEntity<TId> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{this.GetGenericTypeName()}: {Id}";
        }
    }

    public abstract class AppAuditableEntity<TId> : AppEntity<TId>, IAuditableEntity<int>
    {
        protected AppAuditableEntity() : base()
        {
        }

        public int? CreatorId { get; protected set; }
        public int? LastModifyUserId { get; protected set; }
        public DateTimeOffset CreatedTime { get; protected set; }
        public DateTimeOffset? LastModifiedTime { get; protected set; }

        public virtual void UpdateCreatedTime()
        {
            CreatedTime = DateTimeOffset.UtcNow;
        }

        public virtual void SetCreatorId(int? key)
        {
            CreatorId = key;
        }

        public virtual void UpdateLastModifiedTime()
        {
            LastModifiedTime = DateTimeOffset.UtcNow;
        }

        public virtual void SetLastModifyUserId(int? key)
        {
            LastModifyUserId = key;
        }
    }

    public abstract class AppFullAuditableEntity<TId> : AppAuditableEntity<TId>, ISoftDeleteEntity<int>
    {
        public int? DeletorId { get; protected set; }
        public DateTimeOffset? DeletedTime { get; protected set; }
        public bool IsDeleted { get; protected set; }

        public virtual void SetDeletorId(int? key)
        {
            DeletorId = key;
        }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
            DeletedTime = DateTimeOffset.UtcNow;
        }
    }
}
