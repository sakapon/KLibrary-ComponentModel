using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KLibrary
{
    internal static class TypeHelper
    {
        public static object GetDefaultValue(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static object GetDefaultValue(this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var attribute = property.GetCustomAttribute<DefaultValueAttribute>(true);
            return attribute != null ? attribute.Value : property.PropertyType.GetDefaultValue();
        }
    }
}
