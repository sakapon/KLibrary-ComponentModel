using KLibrary.ComponentModel;
using System;
using System.ComponentModel;

namespace $rootnamespace$
{
    // An usage sample.
    public class Person : NotifyBase
    {
        [DefaultValue(-1)]
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DefaultValue("")]
        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DefaultValue("")]
        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DependentOn("FirstName")]
        [DependentOn("LastName")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        }

        public DateTime Birthday
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }
    }
}
