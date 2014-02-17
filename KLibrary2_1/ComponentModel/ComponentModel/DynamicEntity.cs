using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Gets the dictionary which contains the property values of this object.
        /// </summary>
        /// <value>The dictionary which contains the property values of this object.</value>
        protected Dictionary<string, object> PropertyValues { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        public DynamicEntity()
        {
            PropertyValues = new Dictionary<string, object>();
        }

        protected bool GetValue(string propertyName, out object result)
        {
            if (!PropertyValues.ContainsKey(propertyName))
            {
                result = null;
                return false;
            }

            result = PropertyValues[propertyName];
            return true;
        }

        protected bool SetValue(string propertyName, object value)
        {
            if (PropertyValues.ContainsKey(propertyName) && object.Equals(PropertyValues[propertyName], value)) return true;

            PropertyValues[propertyName] = value;
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
            return PropertyValues.Keys;
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
