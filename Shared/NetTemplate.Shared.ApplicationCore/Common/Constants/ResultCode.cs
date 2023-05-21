using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetTemplate.Shared.ApplicationCore.Common.Constants
{
    public enum ResultCode
    {
        #region Common
        Common = 1,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.ObjectResult)]
        Common_ObjectResult = Common + 1,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.NotFound)]
        Common_PostNotFound = Common + 2,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.BadRequest)]
        Common_BadRequest = Common + 3,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.AccessDenied)]
        Common_AccessDenied = Common + 4,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.UnknownError)]
        Common_UnknownError = Common + 5,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.PersistenceError)]
        Common_PersistenceError = Common + 6,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.InvalidPagination)]
        Common_InvalidPagination = Common + 7,

        [Display(GroupName = nameof(Common))]
        [Description(Messages.Common.InvalidData)]
        Common_InvalidData = Common + 8,
        #endregion
    }
}
