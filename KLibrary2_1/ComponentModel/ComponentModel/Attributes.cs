using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Specifies the property on which the current property is dependent.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class DependentOnAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the property on which the current property is dependent.
        /// </summary>
        /// <value>The name of the property on which the current property is dependent.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependentOnAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property on which the current property is dependent.</param>
        public DependentOnAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the map of properties dependency from children to parents.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The map of properties dependency from children to parents.</returns>
        public static Dictionary<string, string[]> GetTargetToSourceMap(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type
                .GetProperties()
                .SelectMany(p => p.GetCustomAttributes<DependentOnAttribute>(true), (p, d) => new { Target = p.Name, Source = d.PropertyName })
                .GroupBy(x => x.Target, x => x.Source)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        /// <summary>
        /// Gets the map of properties dependency from parents to children.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The map of properties dependency from parents to children.</returns>
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
