using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommonMessages = NetTemplate.Shared.ApplicationCore.Common.Constants.Messages;

namespace NetTemplate.Blog.ApplicationCore.Common
{
    public static class Enums
    {
        public enum ResultCode
        {
            #region Common
            Common = 1,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.ObjectResult)]
            Common_ObjectResult = Common + 1,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.NotFound)]
            Common_NotFound = Common + 2,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.BadRequest)]
            Common_BadRequest = Common + 3,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.AccessDenied)]
            Common_AccessDenied = Common + 4,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.UnknownError)]
            Common_UnknownError = Common + 5,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.PersistenceError)]
            Common_PersistenceError = Common + 6,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.InvalidPagination)]
            Common_InvalidPagination = Common + 7,

            [Display(GroupName = nameof(Common))]
            [Description(CommonMessages.InvalidData)]
            Common_InvalidData = Common + 8,
            #endregion
        }
    }
}
