using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KLibrary.ComponentModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class DependentOnAttribute : Attribute
    {
        public string PropertyName { get; private set; }

        public DependentOnAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public static Dictionary<string, string[]> GetTargetToSourceMap(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type
                .GetProperties()
                .SelectMany(p => p.GetCustomAttributes<DependentOnAttribute>(true), (p, d) => new { Target = p.Name, Source = d.PropertyName })
                .GroupBy(x => x.Target, x => x.Source)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public static Dictionary<string, string[]> GetSourceToTargetMap(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type
                .GetProperties()
                .SelectMany(p => p.GetCustomAttributes<DependentOnAttribute>(true), (p, d) => new { Target = p.Name, Source = d.PropertyName })
                .GroupBy(x => x.Source, x => x.Target)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}
