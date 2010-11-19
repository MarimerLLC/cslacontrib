using System;
using System.ComponentModel;
using Csla.Wpf;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using ProjectTracker.Library;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Project
{
    public class ProjectListViewModel :  IActiveAware, INotifyPropertyChanged
    {
        ObjectStatus ProjectsStatus { get; set; }
        private IEventAggregator EventAggregator { get; set; }
        public ProjectListViewModel(IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.ProjectsStatus = new ObjectStatus();
            this.Projects = ProjectTracker.Library.ProjectList.GetProjectList();
            this.EventAggregator.GetEvent<NewProjectAddedEvent>().Subscribe(UpdateProjectList);
        }

        #region Global event handlers
        void UpdateProjectList(object notUsed)
        {
            this.Projects = ProjectTracker.Library.ProjectList.GetProjectList();
        }
        #endregion

        #region Properties
        ProjectList _projects;
        public ProjectList Projects
        {
            get
            {
                return this._projects;
            }
            private set
            {
                if (this._projects == value)
                    return;
                this.ProjectsStatus.DataContext = Projects;
                this._projects = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Projects"));
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
