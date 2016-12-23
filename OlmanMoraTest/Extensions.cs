using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest
{
    public static class Extensions
    {
        public static bool EqualIgnoreCase(this string Origin, string Target)
        {
            return Origin.Equals(Target, StringComparison.CurrentCultureIgnoreCase);
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public static TValue GetValueOrDefault<TKey,TValue>(this Dictionary<TKey, TValue> Dictionary,TKey key, TValue DefaultValue=null) where TValue : class
        {
            TValue value;
            if (Dictionary.TryGetValue(key, out value))
                return value;
            else if (DefaultValue != null)
                return DefaultValue;
            else
                return default(TValue);
        }

        public static string GetFirstOrDefaultValue<TKey>(this Dictionary<TKey, string[]> Dictionary, TKey key) 
        {
            string[] array;
            if (Dictionary.TryGetValue(key, out array))
                return array[0];
            else
                return null;
        }
    }
}
