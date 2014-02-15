using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Provides the base implementation for observable objects.
    /// </summary>
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the dictionary which contains the property values of this object.
        /// </summary>
        /// <value>The dictionary which contains the property values of this object.</value>
        protected Dictionary<string, object> PropertyValues { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyBase"/> class.
        /// </summary>
        protected NotifyBase()
        {
            PropertyValues = GetType()
                .GetRuntimeProperties()
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, TypeHelper.GetDefaultValue);
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        protected T GetValue<T>([CallerMemberName]string propertyName = "")
        {
            if (!PropertyValues.ContainsKey(propertyName)) throw new ArgumentException("A value of the specified property does not exist.", "propertyName");

            return (T)PropertyValues[propertyName];
        }

        /// <summary>
        /// Sets the value of the property.
        /// The <see cref="PropertyChanged"/> event occurs if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="value">The value of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected void SetValue<T>(T value, [CallerMemberName]string propertyName = "")
        {
            var currentValue = GetValue<T>(propertyName);

            if (object.Equals(currentValue, value)) return;
            PropertyValues[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };

        /// <summary>
        /// Notifies that the property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property which has changed.</param>
        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
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
