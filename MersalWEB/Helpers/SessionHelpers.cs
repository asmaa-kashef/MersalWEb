using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace MersalWEB.Helpers
{
    public static class SessionHelpers
    {
        public static void SetObjectAsJson(this ISession session,string key,object value)
        {
            session.SetString(key,JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session,string Key) {
            var value=session.GetString(Key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);       
        }
    }
}
