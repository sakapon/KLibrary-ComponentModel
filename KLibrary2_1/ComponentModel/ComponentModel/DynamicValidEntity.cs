using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace KLibrary.ComponentModel
{
    public class DynamicValidEntity : DynamicEntity, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
