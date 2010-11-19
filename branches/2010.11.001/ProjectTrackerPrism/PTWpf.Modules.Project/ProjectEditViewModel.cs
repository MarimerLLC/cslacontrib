using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Csla.Wpf;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure.RegionContext;
using ProjectTracker.Library;
using PTWpf.Library;
using PTWpf.Library.Contracts;
using PTWpf.Modules.ModuleEvents;
using OutlookStyle.Infrastructure.ApplicationModel;

namespace PTWpf.Modules.Project
{
    /// <summary>
    /// Interaction logic for ProjectEdit.xaml
    /// </summary>
    public partial class ProjectEditViewModel : IRegionContextAware, IActiveAware, INotifyPropertyChanged
    {
        public ObjectStatus ProjectStatus { get; private set; }
        IUnityContainer Container { get; set; }
        IEventAggregator EventAggregator { get; set; }
        IApplicationModel ApplicationModel { get; set; }

        private DelegateCommandReplacement<object> AddNewProjectCommand;
        public DelegateCommandReplacement<object> SaveCommand { get; set; }
        public DelegateCommandReplacement<ProjectTracker.Library.ProjectResource> DeleteCommand { get; set; }
        public DelegateCommandReplacement<object> UndoCommand { get; set; }
        public DelegateCommandReplacement<object> AssignCommand { get; set; }
        public DelegateCommandReplacement<int> OpenResourceCommand { get; set; }
        
        public ProjectEditViewModel(IEventAggregator eventAggregator, IUnityContainer container, IApplicationModel applicationModel)
        {
            this.ApplicationModel = applicationModel;
            this.EventAggregator = eventAggregator;
            this.Container = container;
            this.ProjectStatus = new ObjectStatus();

            // RegionContext is bound to the selected item in the project listbox...
            this.RegionContext = new ObservableObject<object>();
            this.RegionContext.PropertyChanged += new PropertyChangedEventHandler(RegionContext_PropertyChanged);
            
            // watch for login changes....
            this.EventAggregator.GetEvent<ApplyAuthorizationEvent>().Subscribe(ApplyAuthorization);
 
            // register Toolbar global button commmand...
            this.AddNewProjectCommand = new DelegateCommandReplacement<object>(AddNewProject, CanAddNewProject);
            UseCaseCommands.AddNewProjectCommmand.RegisterCommand(AddNewProjectCommand);

            // wire up local button commands....
            this.SaveCommand = new DelegateCommandReplacement<object>(Save, CanSave);
            this.DeleteCommand = new DelegateCommandReplacement<ProjectTracker.Library.ProjectResource>(Delete);
            this.UndoCommand = new DelegateCommandReplacement<object>(Undo, CanUndo);
            this.AssignCommand = new DelegateCommandReplacement<object>(Assign, CanAssign);
            this.OpenResourceCommand = new DelegateCommandReplacement<int>(OpenResourceByID);
        }

        #region Local event handlers
        void RegionContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Guid projectId = Guid.Empty;

            if (RegionContext.Value != null && RegionContext.Value is ProjectInfo)
                projectId = (RegionContext.Value as ProjectInfo).Id;

            if (RegionContext.Value != null && RegionContext.Value is Guid)
                projectId = (Guid)RegionContext.Value;

            this.LoadProject(projectId);

            if (this.Project.IsNew)
                this.Title = string.Format("Project: {0}", "<new>");
            else
                this.Title = string.Format("Project: {0}", this.Project.Name);
        }
        #endregion

        #region Properties
        public ObservableObject<object> RegionContext { get; set; }

        private ProjectTracker.Library.Project _project;
        public ProjectTracker.Library.Project Project
        {
            get
            {
                return this._project;
            }
            set
            {
                this._project = value;

                if (this._project != null)
                {
                    this._project.BeginEdit();
                }

                this.ProjectStatus.DataContext = this._project;

                this.InvokePropertyChanged(new PropertyChangedEventArgs("Project"));
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
                this._title = value;
                this.InvokePropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        #endregion

        #region Global Events

        public void OpenResourceByID(int resourceID)
        {
            this.EventAggregator.GetEvent<OpenResourceByIdEvent>().Publish(resourceID);
        }

        #endregion

        #region Local command handlers
        public void Save(object notUsed)
        {
            bool isNew = this.Project.IsNew;
            this.Project.ApplyEdit();
            var newProjectResource = this.Project.Save();
            this.Project = newProjectResource;
            if (isNew)
                this.EventAggregator.GetEvent<NewProjectAddedEvent>().Publish(null);
        }

        public bool CanSave(object notUsed)
        {
            if (this.Project == null)
                return false;

            return this.Project.IsSavable;

        }

        public void Delete(ProjectTracker.Library.ProjectResource resource)
        {
            this.Project.Resources.Remove(resource);
        }

        public void Undo(object notUsed)
        {
            this.Project.CancelEdit();
            this.Project.BeginEdit();
        }

        public bool CanUndo(object notUsed)
        {
            if (this.Project == null)
                return false;

            return this.Project.IsDirty;
        }

        public void Assign(object notUsed)
        {
            IResourceAssignService pas = this.Container.Resolve<IResourceAssignService>();
            pas.Assign(this.Project);
        }

        public bool CanAssign(object notUsed)
        {
            if (this.ProjectStatus == null)
                return false;

            return this.ProjectStatus.CanEditObject;
        }
        #endregion

        #region Global Event handler

        private void AddNewProject(object notUsed)
        {
            this.EventAggregator.GetEvent<OpenProjectByIdEvent>().Publish(Guid.Empty);
        }

        private bool CanAddNewProject(object notUsed)
        {
            return this.ProjectStatus.CanCreateObject;
        }

        void ApplyAuthorization(object notUsed)
        {
            this.ProjectStatus.Refresh();
        }


        void LoadProject(Guid projectID)
        {
            if (projectID == null || projectID == Guid.Empty)
                this.Project = ProjectTracker.Library.Project.NewProject();
            else
                this.Project = ProjectTracker.Library.Project.GetProject(projectID);
            this.IsActive = true;
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