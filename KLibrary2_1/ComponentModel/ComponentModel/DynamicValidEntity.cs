using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace KLibrary.ComponentModel
{
    public class DynamicValidEntity : DynamicObject, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public void DefineProperty(string name, Func<object, bool> validate, object initialValue)
        {
            throw new NotImplementedException();
        }

        public void DefineGetProperty(string name, Func<dynamic, object> getValue)
        {
            throw new NotImplementedException();
        }

        public void DefinePropertiesDependency(string sourceProperty, string targetProperty)
        {
            throw new NotImplementedException();
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
