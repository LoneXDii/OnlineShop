using System.ComponentModel;

namespace OrderService.Application.Extensions;

public static class EnumExtension
{
	public static string GetDescription<T>(this T value) where T : Enum
	{
		var field = value.GetType().GetField(value.ToString());
		var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
		return attribute != null ? attribute.Description : value.ToString();
	}
}