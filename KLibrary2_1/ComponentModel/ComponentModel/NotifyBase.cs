using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KLibrary.ComponentModel
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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
