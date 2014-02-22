using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace KLibrary.ComponentModel
{
    public class DynamicValidEntity : DynamicObject, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected Dictionary<string, Property> Properties { get; private set; }

        protected struct Property
        {
            public string Name;
            public object Value;
            public Func<object, bool> Validate;
            public Func<dynamic, object> GetValue;
            public List<string> InfluencedProperties;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicValidEntity"/> class.
        /// </summary>
        public DynamicValidEntity()
        {
            Properties = new Dictionary<string, Property>();
        }

        public void DefineProperty(string name, Func<object, bool> validate, object initialValue)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The value is empty.", "name");
            if (Properties.ContainsKey(name)) throw new ArgumentException("The property is already defined.", "name");
            if (validate == null) throw new ArgumentNullException("validate");
            if (!validate(initialValue)) throw new ArgumentException("The initial value is not valid.", "initialValue");

            Properties[name] = new Property
            {
                Name = name,
                Value = initialValue,
                Validate = validate,
            };
        }

        public void DefineGetProperty(string name, Func<dynamic, object> getValue)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The value is empty.", "name");
            if (Properties.ContainsKey(name)) throw new ArgumentException("The property is already defined.", "name");
            if (getValue == null) throw new ArgumentNullException("getValue");

            Properties[name] = new Property
            {
                Name = name,
                GetValue = getValue,
            };
        }

        public void DefinePropertiesDependency(string sourceProperty, string targetProperty)
        {
            if (!Properties.ContainsKey(sourceProperty)) throw new ArgumentException("The property is not defined.", "sourceProperty");
            if (!Properties.ContainsKey(targetProperty)) throw new ArgumentException("The property is not defined.", "targetProperty");

            var property = Properties[sourceProperty];
            if (property.InfluencedProperties == null)
            {
                property.InfluencedProperties = new List<string>();
            }
            if (property.InfluencedProperties.Contains(targetProperty)) throw new ArgumentException("The dependency is already defined.", "targetProperty");

            property.InfluencedProperties.Add(targetProperty);
            Properties[sourceProperty] = property;
        }

        protected bool GetValue(string propertyName, out object result)
        {
            if (!Properties.ContainsKey(propertyName))
            {
                result = null;
                return false;
            }

            var property = Properties[propertyName];
            result = property.GetValue == null ? property.Value : property.GetValue(this);
            return true;
        }

        protected bool SetValue(string propertyName, object value)
        {
            if (!Properties.ContainsKey(propertyName)) return false;
            var property = Properties[propertyName];
            if (property.GetValue != null) return false;
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
                    NotifyPropertyChanged(target);
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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = (o, e) => { };

        public void NotifyErrorsChanged(string propertyName)
        {
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddErrorsChangedHandler(string propertyName, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            ErrorsChanged += (o, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    action();
                }
            };
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasErrors
        {
            get { throw new NotImplementedException(); }
        }
    }
}
