using System.ComponentModel;
using System.Reflection;

namespace CommUnity.FrontEnd.Helpers
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<T>(this T enumValue) where T : Enum
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute? attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                   .Cast<DescriptionAttribute>()
                                                   .FirstOrDefault();
            return attribute?.Description ?? enumValue!.ToString();
        }
    }
}
