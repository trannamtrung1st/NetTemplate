using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetTemplate.Common.Json.Utils
{
    public static class JsonHelper
    {
        public static readonly JsonSerializerSettings DefaultCamelCase;

        static JsonHelper()
        {
            DefaultCamelCase = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
