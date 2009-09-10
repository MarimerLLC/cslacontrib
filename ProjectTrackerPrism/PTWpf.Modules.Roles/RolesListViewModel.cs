using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Csla.Wpf;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using ProjectTracker.Library.Admin;
using PTWpf.Library;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Roles
{
    public class RolesListViewModel : IActiveAware, INotifyPropertyChanged
    {
        public ObjectStatus RoleListStatus { get; private set; }
        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;
        public DelegateCommandReplacement<object> CreateNewRoleCommand { get; set; }
        public DelegateCommandReplacement<object> SaveCommand { get; set; }
        public DelegateCommandReplacement<Role> DeleteCommand { get; set; }
        public DelegateCommandReplacement<object> UndoCommand { get; set; }
        public RolesListViewModel(IEventAggregator eventAggregator, IUnityContainer container)
        {
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<ApplyAuthorizationEvent>().Subscribe(ApplyAuthorization);
            this._container = container;
            this.CreateNewRoleCommand = new DelegateCommandReplacement<object>(CreateNewRole, CanCreateNewRole);
            UseCaseCommands.CreateNewRoleCommmand.RegisterCommand(this.CreateNewRoleCommand);
            this.SaveCommand = new DelegateCommandReplacement<object>(Save, CanSave);
            this.DeleteCommand = new DelegateCommandReplacement<Role>(Delete);
            this.UndoCommand = new DelegateCommandReplacement<object>(Undo, CanUndo);
            this.RoleListStatus = new ObjectStatus();
        }

        #region Global event handlers

        void ApplyAuthorization(object notUsed)
        {
            this.RoleListStatus.Refresh();
        }

        #endregion

        #region Local commands

        private void CreateNewRole(object obj)
        {
            if(this.RoleList!=null)
                this.RoleList.AddNew();
        }

        private bool CanCreateNewRole(object obj)
        {
            return this.RoleListStatus.CanCreateObject;
        }

        public void Save(object notUsed)
        {
            this.RoleList.ApplyEdit();
            var newProjectResource = this.RoleList.Save();
            this.RoleList = newProjectResource;
        }

        public bool CanSave(object notUsed)
        {
            if (this.RoleList == null)
                return false;

            return this.RoleList.IsSavable;

        }

        public void Delete(Role role)
        {
            this.RoleList.Remove(role);
        }

        public void Undo(object notUsed)
        {
            this.RoleList.CancelEdit();
            this.RoleList.BeginEdit();
        }

        public bool CanUndo(object notUsed)
        {
            if (this.RoleList == null)
                return false;

            return this.RoleList.IsDirty;
        }
        #endregion

        #region Properties

        private ProjectTracker.Library.Admin.Roles _roleList;
        public ProjectTracker.Library.Admin.Roles RoleList
        {
            get
            {
                if (this._roleList == null)
                {
                    this._roleList = ProjectTracker.Library.Admin.Roles.GetRoles();
                    this._roleList.BeginEdit();
                    RoleListStatus.DataContext = this._roleList;
                }

                return this._roleList;
            }
            set
            {
                this._roleList = value;

                if (this._roleList != null)
                {
                    this._roleList.BeginEdit();
                }

                RoleListStatus.DataContext = this._roleList;

                this.InvokePropertyChanged(new PropertyChangedEventArgs("RoleList"));
            }
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
