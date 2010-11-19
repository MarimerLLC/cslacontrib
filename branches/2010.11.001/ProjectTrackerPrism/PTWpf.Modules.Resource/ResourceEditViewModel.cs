using System;
using System.ComponentModel;
using Csla.Wpf;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Events;
using ProjectTracker.Library;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Library;
using OutlookStyle.Infrastructure.RegionContext;
using PTWpf.Library.Contracts;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure.ApplicationModel;

namespace PTWpf.Modules.Resource
{
    public class ResourceEditViewModel : IRegionContextAware, IActiveAware, INotifyPropertyChanged
    {
        public ObjectStatus ProjectResourceStatus { get; private set; }
        private IEventAggregator EventAggregator { get; set; }
        public DelegateCommandReplacement<object> SaveCommand { get; set; }
        public DelegateCommandReplacement<ProjectTracker.Library.ResourceAssignment> DeleteCommand { get; set; }
        public DelegateCommandReplacement<object> UndoCommand { get; set; }
        public DelegateCommandReplacement<object> AssignCommand { get; set; }
        public DelegateCommandReplacement<Guid> OpenProjectCommand { get; set; }

        private DelegateCommandReplacement<object> AddNewResourceCommmand;
        
        IUnityContainer Container { get; set; }
        private IApplicationModel ApplicationModel { get; set; }

        public ResourceEditViewModel(IEventAggregator eventAggregator, IUnityContainer container, IApplicationModel applicationModel)
        {
            this.ProjectResourceStatus = new ObjectStatus();
            this.EventAggregator = eventAggregator;
            this.Container = container;
            this.ApplicationModel = applicationModel;
   
            this.RegionContext = new ObservableObject<object>();
            this.RegionContext.PropertyChanged += new PropertyChangedEventHandler(RegionContext_PropertyChanged);

            // register Toolbar global button commmand...
            this.AddNewResourceCommmand = new DelegateCommandReplacement<object>(AddNewResource, CanAddNewResource);
            UseCaseCommands.AddNewResourceCommmand.RegisterCommand(this.AddNewResourceCommmand);

            // watch for login changes....
            this.EventAggregator.GetEvent<ApplyAuthorizationEvent>().Subscribe(ApplyAuthorization);

            // wire up local button commands....
            this.SaveCommand = new DelegateCommandReplacement<object>(Save, CanSave);
            this.DeleteCommand = new DelegateCommandReplacement<ProjectTracker.Library.ResourceAssignment>(Delete);
            this.UndoCommand = new DelegateCommandReplacement<object>(Undo, CanUndo);
            this.AssignCommand = new DelegateCommandReplacement<object>(Assign, CanAssign);
            this.OpenProjectCommand = new DelegateCommandReplacement<Guid>(OpenProjectById);
        }


        #region Local event handlers
        void RegionContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int resourceId = 0;

            if(RegionContext.Value != null && RegionContext.Value is ResourceInfo)
                resourceId = (RegionContext.Value as ResourceInfo).Id;

            if (RegionContext.Value != null && RegionContext.Value is int)
                resourceId = (int) RegionContext.Value;

            LoadResource(resourceId);
        }
        #endregion

        #region Global event handlers
        void ApplyAuthorization(object notUsed)
        {
            this.ProjectResourceStatus.Refresh();
        }

        private void LoadResource(int resourceId)
        {
            if (resourceId == 0)
                this.ProjectResource = ProjectTracker.Library.Resource.NewResource();
            else
            {
                this.ProjectResource = ProjectTracker.Library.Resource.GetResource(resourceId);
            }

            if (this.ProjectResource.IsNew)
                this.Title = string.Format("Resource: {0}", "<new>");
            else
                this.Title = string.Format("Resource: {0}", this.ProjectResource.FullName);
        }
        #endregion

        #region Properties
        public ObservableObject<object> RegionContext { get; set; }

        private ProjectTracker.Library.Resource _projectResource;
        public ProjectTracker.Library.Resource ProjectResource
        {
            get
            {
                return this._projectResource;
            }
            set
            {
                this._projectResource = value;
                
                if (this.ProjectResource != null)
                {
                    this._projectResource.BeginEdit();
                }

                this.ProjectResourceStatus.DataContext = this._projectResource;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("ProjectResource"));
            }
        }

        private RoleList _roleList;
        public RoleList RoleList
        {
            get
            {
                if (this._roleList == null)
                    this._roleList = ProjectTracker.Library.RoleList.GetList();
                return this._roleList;
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (this._title == value)
                    return;

                this._title = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Title"));
            }


        }
        #endregion

        #region Local command handlers
        private void AddNewResource(object obj)
        {
            this.EventAggregator.GetEvent<OpenResourceByIdEvent>().Publish(0);
        }

        private bool CanAddNewResource(object arg)
        {
            return ProjectResourceStatus.CanEditObject;
        }

        public void Save(object notUsed)
        {
            bool isNew = this.ProjectResource.IsNew;
            this.ProjectResource.ApplyEdit();
            var newProjectResource = this.ProjectResource.Save();
            this.ProjectResource = newProjectResource;
            if(isNew)
                this.EventAggregator.GetEvent<NewResourceAddedEvent>().Publish(null);
        }

        public bool CanSave(object notUsed)
        {
            if(this.ProjectResource==null)
                return false;

           return this.ProjectResource.IsSavable;

        }

        public void Delete(ProjectTracker.Library.ResourceAssignment assignment)
        {
            this.ProjectResource.Assignments.Remove(assignment);
        }

        public void Undo(object notUsed)
        {
            this.ProjectResource.CancelEdit();
            this.ProjectResource.BeginEdit();
        }

        public bool CanUndo(object notUsed)
        {
            if (this.ProjectResource == null)
                return false;
            
            return this.ProjectResource.IsDirty;
        }
        public void Assign(object notUsed)
        {
            IProjectAssignService pas = this.Container.Resolve<IProjectAssignService>();
            pas.Assign(this.ProjectResource);
        }
        public bool CanAssign(object notUsed)
        {
            if (this.ProjectResourceStatus == null)
                return false;

            return this.ProjectResourceStatus.CanEditObject;
        }
        #endregion

        #region Global Events
        public void OpenProjectById(Guid projectID)
        {
            this.EventAggregator.GetEvent<OpenProjectByIdEvent>().Publish(projectID);
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
