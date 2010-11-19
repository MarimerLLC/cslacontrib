using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Library;

namespace PTWpf.Modules.Login
{
    public class LoginViewModel : INotifyPropertyChanged, IActiveAware
    {
        public DelegateCommandReplacement<object> LoginCommand { get; private set; }
        public DelegateCommand<object> LogoutCommand { get; private set; }
        private IEventAggregator EventAggregator { get; set; }
        public LoginViewModel(IEventAggregator eventAggregator)
        {
            this.LoginCommand = new DelegateCommandReplacement<object>(Login, CanLogin);
            this.LogoutCommand = new DelegateCommand<object>(Logout, CanLogout);
            UseCaseCommands.LogoutCommand.RegisterCommand(this.LogoutCommand);
            this.EventAggregator = eventAggregator;
        }

        private string _username;
        public string Username
        {
            get { return this._username;  }
            set
            {
                if(this._username==value)
                    return;

                this._username = value;
                this.OnPropertyChanged("Password");
            }
        }

        private string _password;
        public string Password
        {
            get { return this._password; }
            set
            {
                if (this._password == value)
                    return;

                this._password = value;
                this.OnPropertyChanged("Password");
            }
        }
        #region Login/Logout
        void Login(object notUsed)
        {
            if (Csla.ApplicationContext.User.Identity.IsAuthenticated)
            {
                ProjectTracker.Library.Security.PTPrincipal.Logout();
                this.EventAggregator.GetEvent<StatusbarMessageEvent>().Publish("Not logged in");
            }

            ProjectTracker.Library.Security.PTPrincipal.Login(this.Username, this.Password);
            if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
            {
                ProjectTracker.Library.Security.PTPrincipal.Logout();
                this.EventAggregator.GetEvent<StatusbarMessageEvent>().Publish("Not logged in");
            }
            else
            {
                this.EventAggregator.GetEvent<StatusbarMessageEvent>().Publish(string.Format("Logged in as {0}", Csla.ApplicationContext.User.Identity.Name));
            }
            this.EventAggregator.GetEvent<ApplyAuthorizationEvent>().Publish(null);
        }

        bool CanLogin(object notUsed)
        {
            if(!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.Password))
                return true;
            else
                return false;     
        }

        void Logout(object notUsed)
        {
            this.EventAggregator.GetEvent<StatusbarMessageEvent>().Publish("Not logged in");

            if (Csla.ApplicationContext.User.Identity.IsAuthenticated)
            {
                ProjectTracker.Library.Security.PTPrincipal.Logout();
            }
        }

        bool CanLogout(object notUsed)
        {
            return Csla.ApplicationContext.User.Identity.IsAuthenticated;
        }
        #endregion

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, e);
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.InvokePropertyChanged(new PropertyChangedEventArgs(propertyName));
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
