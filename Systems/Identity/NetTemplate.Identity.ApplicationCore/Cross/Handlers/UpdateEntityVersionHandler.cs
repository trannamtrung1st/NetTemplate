using NetTemplate.Shared.ApplicationCore.Common.Interfaces;

namespace NetTemplate.Identity.ApplicationCore.Cross.Handlers
{
    public class UpdateEntityVersionHandler
    {
        private readonly IEntityVersionManager _manager;

        public UpdateEntityVersionHandler(IEntityVersionManager manager)
        {
            _manager = manager;
        }
    }
}
