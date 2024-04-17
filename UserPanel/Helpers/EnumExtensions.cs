using System.Reflection;
using UserPanel.Attributes;
namespace UserPanel.Helpers
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttribute<StringValueAttribute>();
            return attribute != null ? attribute.Value : value.ToString();
        }
    }
}
