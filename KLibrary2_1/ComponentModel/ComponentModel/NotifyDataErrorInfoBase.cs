using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KLibrary.ComponentModel
{
    /// <summary>
    /// Provides the base implementation for the <see cref="INotifyPropertyChanged"/> and <see cref="INotifyDataErrorInfo"/> interfaces.
    /// </summary>
    public abstract class NotifyDataErrorInfoBase : NotifyPropertyChangedBase, INotifyDataErrorInfo
    {
        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = (o, e) => { };

        /// <summary>
        /// Notifies that the validation errors have changed.
        /// </summary>
        /// <param name="propertyName">The name of the property; or null or String.Empty for entity-level errors.</param>
        public virtual void NotifyErrorsChanged([CallerMemberName]string propertyName = "")
        {
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Add the action which is executed when the validation errors have changed.
        /// </summary>
        /// <param name="propertyName">The name of the property; or null or String.Empty for entity-level errors.</param>
        /// <param name="action">The action which is executed when the validation errors have changed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        public virtual void AddErrorsChangedHandler(string propertyName, Action action)
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

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or String.Empty, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public abstract System.Collections.IEnumerable GetErrors(string propertyName);

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <value>true if the entity currently has validation errors; otherwise, false.</value>
        public abstract bool HasErrors { get; }
    }
}
