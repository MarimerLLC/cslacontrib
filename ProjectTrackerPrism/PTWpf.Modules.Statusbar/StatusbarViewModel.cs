using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Presentation.Events;

namespace PTWpf.Modules.Statusbar
{
    public class StatusbarViewModel : INotifyPropertyChanged, IActiveAware
    {
        public StatusbarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<PTWpf.Modules.ModuleEvents.StatusbarMessageEvent>().Subscribe(SetMessage, ThreadOption.UIThread);
        }

        private string _message = "Not logged in";
        public string Message
        {
            get { return this._message; }
            set
            {
                if(this._message == value)
                    return;
            
                this._message = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }
        void SetMessage(string message)
        {
            this.Message = message;
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
