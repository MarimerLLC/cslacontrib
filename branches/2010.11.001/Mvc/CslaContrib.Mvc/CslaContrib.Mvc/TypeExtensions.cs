using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CslaContrib.Mvc
{
    static class TypeExtensions
    {
        public static Type GetPropertyType(this Type type, string propName)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(type))
            {
                if (propName.Equals(prop.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return prop.PropertyType;
                }
            }
            return null;
        }

        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>));
        }

        public static IEnumerable<Type> GetTypes(this object[] values)
        {
            if (values.Length > 0)
            {
                int i = 0;
                yield return values[i++].GetType();
            }
        }
    }
}
