using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Provides the base implementation for observable objects.
    /// </summary>
    public abstract class NotifyBase : INotifyPropertyChanged
    {
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
