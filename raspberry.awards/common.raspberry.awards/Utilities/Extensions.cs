namespace common.raspberry.awards.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Reflection;

    public static class Extensions
    {
        public static string ToStringObject(this object value, string property)
        {
            PropertyInfo pi = value.GetType().GetProperty(property);
            return (string)(pi.GetValue(value, null));
        }

        public static object ToJasonObject(this string value)
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value.Replace('=', ':')));
        }

        public static string ToSerializableObject(this string value, string oldString, string newString)
        {
            return value.Replace(oldString, newString);
        }

        public static DateTime ToDateTime(this string value)
        {
            return Convert.ToDateTime(value);
        }

        public static DateTime? ToNullableDateTime(this string value)
        {
            return (DateTime?)Convert.ToDateTime(value);
        }
    }
}
