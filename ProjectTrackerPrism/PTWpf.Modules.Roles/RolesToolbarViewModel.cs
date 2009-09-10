using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using PTWpf.Library;
using PTWpf.Modules.ModuleEvents;
using ProjectTracker.Library.Admin;
using Csla.Wpf;

namespace PTWpf.Modules.Roles
{
    public class RolesToolbarViewModel : INotifyPropertyChanged, IActiveAware
    {
        public DelegateCommandReplacement<object> CreateNewRoleCommand { get; private set; }

        public RolesToolbarViewModel()
        {
            this.CreateNewRoleCommand = new DelegateCommandReplacement<object>(CreateNewRole, CanCreateNewRole);
        }

        private void CreateNewRole(object notUsed)
        {
            UseCaseCommands.CreateNewRoleCommmand.Execute(null);
        }

        private bool CanCreateNewRole(object notUsed)
        {
            return UseCaseCommands.CreateNewRoleCommmand.CanExecute(null);
        }

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
