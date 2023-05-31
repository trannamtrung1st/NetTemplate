namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public interface IByCreatorQuery<TUserId> where TUserId : struct
    {
        TUserId? CreatorId { get; set; }
    }
}
