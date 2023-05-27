using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetTemplate.Shared.ApplicationCore.Common;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;
using NetTemplate.Shared.ApplicationCore.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetTemplate.Shared.WebApi.Common.Models
{
    public class ApiResponse
    {
        private ResultCode _resultCode;

        protected ApiResponse(ResultCode code, IEnumerable<string> messages = null, object data = null)
        {
            _resultCode = code;
            _messages = messages;
            Data = data;
        }

        public int Code => _resultCode.Code;

        private IEnumerable<string> _messages;
        public IEnumerable<string> Messages
        {
            get
            {
                if (_messages != null) return _messages;

                return new[] { _resultCode.Description };
            }
        }

        public object Data { get; }

        [JsonExtensionData]
        public IDictionary<string, JToken> Extensions { get; set; } = new Dictionary<string, JToken>();

        public static ApiResponse Object(object data, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.Common.ObjectResult, messages, data);

        public static ApiResponse BadRequest(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.Common.BadRequest, messages, data);

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
            => new ApiResponse(ResultCodes.Common.NotFound, messages, data);

        public static ApiResponse UnknownError(object data = null, IEnumerable<string> messages = null)
            => new ApiResponse(ResultCodes.Common.UnknownError, messages, data);
    }
}
