using Newtonsoft.Json;

namespace MvcCoreSessionEmpleados.Extensions
{
    public static class SessionExtension
    {
        public static T? GetObject<T>(this ISession session, string key)
        {
            string? json = session.GetString(key);

            if (json == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            string data = JsonConvert.SerializeObject(value);

            session.SetString(key, data);
        }
    }
}