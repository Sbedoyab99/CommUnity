using System.ComponentModel;
using System.Reflection;
using static MudBlazor.Colors;

namespace CommUnity.FrontEnd.Helpers
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription<T>(this T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString())!;
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
