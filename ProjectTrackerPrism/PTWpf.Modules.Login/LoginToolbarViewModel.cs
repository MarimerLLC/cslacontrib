using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Csla.Wpf;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using PTWpf.Library;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Login
{
    public class LoginToolbarViewModel : INotifyPropertyChanged, IActiveAware
    {
        public DelegateCommandReplacement<object> LogoutCommand { get; private set; }

        public LoginToolbarViewModel(IEventAggregator eventAggregator)
        {
            this.LogoutCommand = new DelegateCommandReplacement<object>(Logout, CanLogout);
        }

        #region Local commands
        void Logout(object notUsed)
        {
            UseCaseCommands.LogoutCommand.Execute(null);
        }

        bool CanLogout(object notUsed)
        {
            return UseCaseCommands.LogoutCommand.CanExecute(null);
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
