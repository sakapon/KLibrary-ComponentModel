using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Represents the observable object for the dynamic programming.
    /// </summary>
    public class DynamicEntity : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the dictionary which contains the properties of this object.
        /// </summary>
        /// <value>The dictionary which contains the properties of this object.</value>
        protected Dictionary<string, Property> Properties { get; private set; }

        [DebuggerDisplay(@"\{{Name}\}")]
        protected struct Property
        {
            public string Name;
            public object Value;
            public Func<dynamic, object> GetValue;
            public HashSet<Property> InfluencingProperties;
            public HashSet<Property> InfluencedProperties;

            public bool IsReadOnly
            {
                get { return GetValue != null; }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        public DynamicEntity()
        {
            Properties = new Dictionary<string, Property>();
        }

        public void DefineProperty(string name, object initialValue)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The value is empty.", "name");
            if (Properties.ContainsKey(name)) throw new ArgumentException("The property is already defined.", "name");

            Properties[name] = new Property
            {
                Name = name,
                Value = initialValue,
            };
        }

        public void DefineGetProperty(string name, Func<dynamic, object> getValue, params string[] sourceProperties)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The value is empty.", "name");
            if (Properties.ContainsKey(name)) throw new ArgumentException("The property is already defined.", "name");
            if (getValue == null) throw new ArgumentNullException("getValue");
            if (sourceProperties == null) sourceProperties = new string[0];

            Properties[name] = new Property
            {
                Name = name,
                GetValue = getValue,
            };

            foreach (var source in sourceProperties)
            {
                DefinePropertiesDependency(source, name);
            }
        }

        public void DefinePropertiesDependency(string sourceProperty, string targetProperty)
        {
            if (!Properties.ContainsKey(sourceProperty)) throw new ArgumentException("The property is not defined.", "sourceProperty");
            if (!Properties.ContainsKey(targetProperty)) throw new ArgumentException("The property is not defined.", "targetProperty");

            var source = Properties[sourceProperty];
            var target = Properties[targetProperty];

            if (source.InfluencedProperties == null)
            {
                source.InfluencedProperties = new HashSet<Property>();
            }
            if (source.InfluencedProperties.Contains(target)) return;

            if (target.InfluencingProperties == null)
            {
                target.InfluencingProperties = new HashSet<Property>();
            }
            if (target.InfluencingProperties.Contains(source)) return;

            source.InfluencedProperties.Add(target);
            target.InfluencingProperties.Add(source);

            Properties[sourceProperty] = source;
            Properties[targetProperty] = target;
        }

        protected bool GetValue(string propertyName, out object result)
        {
            if (!Properties.ContainsKey(propertyName))
            {
                result = null;
                return false;
            }

            var property = Properties[propertyName];
            result = property.IsReadOnly ? property.GetValue(this) : property.Value;
            return true;
        }

        protected bool SetValue(string propertyName, object value)
        {
            if (!Properties.ContainsKey(propertyName)) return false;
            var property = Properties[propertyName];
            if (property.IsReadOnly) return false;
            if (object.Equals(property.Value, value)) return true;

            property.Value = value;
            Properties[propertyName] = property;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return GetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return SetValue(binder.Name, value);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null || indexes.Length < 1 || !(indexes[0] is string))
            {
                result = null;
                return false;
            }
            return GetValue((string)indexes[0], out result);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null || indexes.Length < 1 || !(indexes[0] is string))
            {
                return false;
            }
            return SetValue((string)indexes[0], value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Properties.Keys;
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };

        /// <summary>
        /// Notifies that the property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property which has changed.</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (Properties.ContainsKey(propertyName) && Properties[propertyName].InfluencedProperties != null)
            {
                foreach (var target in Properties[propertyName].InfluencedProperties)
                {
                    NotifyPropertyChanged(target.Name);
                }
            }
        }

        /// <summary>
        /// Add the action which is executed when the property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property which has changed.</param>
        /// <param name="action">The action which is executed when the property has changed.</param>
        public void AddPropertyChangedHandler(string propertyName, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    action();
                }
            };
        }
    }
}
