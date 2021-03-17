using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MovieDatabase.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription <TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return enumValue.ToString();
        }
    }
}
