namespace NetTemplate.Shared.ApplicationCore.Common.Interfaces
{
    public interface IByCreatorQuery<TUserId> where TUserId : struct
    {
        TUserId? CreatorId { get; set; }
    }
}
