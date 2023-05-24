using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetTemplate.Common.Enumerations;
using NetTemplate.Shared.ApplicationCore.Common.Constants;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetTemplate.Shared.WebApi.Models
{
    public class ApiResponse
    {
        protected ApiResponse(ResultCode code, IEnumerable<string> messages = null, object data = null)
        {
            Code = code;
            _messages = messages;
            Data = data;
        }

        public ResultCode Code { get; }

        private IEnumerable<string> _messages;
        public IEnumerable<string> Messages
        {
            get
            {
                if (_messages != null) return _messages;

                string represent = Code.GetDescription() ?? Code.GetDisplayName() ?? Code.GetName();

                return new[] { represent };
            }
        }

        public object Data { get; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; } = new Dictionary<string, JToken>();

        public static ApiResponse Object(object data, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCode.Common_ObjectResult, messages, data);

        public static ApiResponse BadRequest(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCode.Common_BadRequest, messages, data);

        public static ApiResponse BadRequest(ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                .Select(o => o.ErrorMessage)
                .ToArray();
            var apiResponse = BadRequest(validationErrors);
            return apiResponse;
        }

        public static ApiResponse BadRequest(ModelStateDictionary modelState)
        {
            var validationErrors = modelState.Values
                .SelectMany(o => o.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();
            var apiResponse = BadRequest(validationErrors);
            return apiResponse;
        }

        public static ApiResponse Exception(BaseException exception)
            => new ApiResponse(exception.Code, exception.Messages, exception.DataObject);

        public static ApiResponse NotFound(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCode.Common_NotFound, messages, data);

        public static ApiResponse UnknownError(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCode.Common_UnknownError, messages, data);
    }
}
