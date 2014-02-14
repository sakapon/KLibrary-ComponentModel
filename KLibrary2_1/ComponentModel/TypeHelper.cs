using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
