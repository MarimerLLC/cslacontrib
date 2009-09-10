using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using ProjectTracker.Library;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Library;

namespace PTWpf.Modules.Resource
{
    public class ResourceListViewModel :  IActiveAware, INotifyPropertyChanged
    {
        private IEventAggregator EventAggregator { get; set;}
        private IApplicationModel ApplicationModel { get; set; }
        IUnityContainer Container { get; set; }
        public ResourceListViewModel(IEventAggregator eventAggregator, IUnityContainer container, IApplicationModel applicationModel)
        {
            this.EventAggregator = eventAggregator;
            this.Container = container;
            this.ApplicationModel = applicationModel;
            // load resource list...
            this.Resources = ProjectTracker.Library.ResourceList.GetResourceList();
            // watch for new added resources...
            this.EventAggregator.GetEvent<NewResourceAddedEvent>().Subscribe(UpdateResourceList);
        }

        #region Global event handlers
        void UpdateResourceList(object notUsed)
        {
            this.Resources = ProjectTracker.Library.ResourceList.GetResourceList();
        }
        #endregion

        #region Properties
        ResourceList _resources;
        public ResourceList Resources
        {
            get
            {
                return this._resources;
            }
            private set
            {
                if (this._resources == value)
                    return;
                this._resources = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Resources"));
            }

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

        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        protected void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, e);
        }

        #endregion
    }
}
