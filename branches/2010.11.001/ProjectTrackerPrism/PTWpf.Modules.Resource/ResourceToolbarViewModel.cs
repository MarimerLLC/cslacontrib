using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Csla.Wpf;
using PTWpf.Library;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Resource
{
    public class ResourceToolbarViewModel : INotifyPropertyChanged, IActiveAware
    {
        public DelegateCommandReplacement<object> AddNewResourceCommand { get; private set; }
        public ResourceToolbarViewModel()
        {
            this.AddNewResourceCommand = new DelegateCommandReplacement<object>(AddNewResource, CanAddNewResource);
        }

        #region Local Commands
        void AddNewResource(object notUsed)
        {
            UseCaseCommands.AddNewResourceCommmand.Execute(null);
        }

        bool CanAddNewResource(object notUsed)
        {
            return UseCaseCommands.AddNewResourceCommmand.CanExecute(null);
        }
        #endregion

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, e);
        }

        #endregion

        #region IActiveAware Member

        private bool _isActive;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (value == this._isActive)
                    return;

                this._isActive = value;
                this.InvokeIsActiveChanged(EventArgs.Empty);
                this.InvokePropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }
        }

        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler changed = IsActiveChanged;
            if (changed != null) changed(this, e);
        }

        #endregion
    }
}
