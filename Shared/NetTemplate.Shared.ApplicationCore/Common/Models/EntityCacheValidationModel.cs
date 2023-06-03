namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public class EntityCacheValidationModel<TData>
    {
        public bool NeedUpdate { get; set; }
        public TData ExtraData { get; set; }
    }
}
