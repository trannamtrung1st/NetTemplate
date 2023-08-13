using Newtonsoft.Json;

namespace NetTemplate.Common.Objects
{
    public static class ObjectUtils
    {
        public static T JsonDeepClone<T>(this T srcObj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(srcObj));
        }
    }
}
