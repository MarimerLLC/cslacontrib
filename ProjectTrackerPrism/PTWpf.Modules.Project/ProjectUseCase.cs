using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using Microsoft.Practices.Composite.Regions;
using PTWpf.Library.Contracts;
using PTWpf.Project.Modules;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Project
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class ProjectUseCase : ActiveAwareUseCaseController
    {
        private IApplicationModel ApplicationModel { get; set; }
        IUnityContainer Container { get; set; }
        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private ProjectListViewModel _projectListViewModel;
        private ProjectToolbarViewModel _projectToolbarViewModel;

        public ProjectUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            , IRegionManager regionManager
            , IUnityContainer container
            , IEventAggregator eventAggregator
            // Get the factories that can create the viewmodels
            , ObjectFactory<ProjectListViewModel> ProjectViewModel
            , ObjectFactory<ProjectToolbarViewModel> ProjectToolbarViewModel
            , IApplicationModel applicationModel
            , IModelVisualizationRegistry modelVisualizationRegistry)
           : base(viewtoToRegionBinder)
        {
            this.ApplicationModel = applicationModel;
            this.Container = container;

            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
               () => this._projectListViewModel = ProjectViewModel.CreateInstance()
               , () => this._projectToolbarViewModel = ProjectToolbarViewModel.CreateInstance());

            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            modelVisualizationRegistry.Register<ProjectListViewModel, ProjectListView>();
            modelVisualizationRegistry.Register<ProjectToolbarViewModel, ProjectToolbarView>();
            modelVisualizationRegistry.Register<ProjectEditViewModel, ProjectEditView>();

            // Register Dialog Popup service...
            container.RegisterType<IProjectAssignService, ProjectAssignmentService>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(container.Resolve<IProjectAssignService>());

            regionManager.RegisterViewWithRegion("ProjectEditRegion", typeof(ProjectEditViewModel));

            // watch for OpenProjectEvents fired by (Link)Button command in ResourceEditViewModel...
            eventAggregator.GetEvent<OpenProjectByIdEvent>().Subscribe(OpenProjectById);
        }

        public override string Name
        {
            get { return "Projects"; }
        }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("MainRegion", this._projectListViewModel);
            this.ViewToRegionBinder.Add("ToolbarRegion", this._projectToolbarViewModel);
        }

        void OpenProjectById(Guid projectId)
        {
            NewProjectUseCase newProjectUseCase = this.Container.Resolve<NewProjectUseCase>();
            newProjectUseCase.ProjectId = projectId;
            ApplicationModel.AddMainUseCase(newProjectUseCase);
            ApplicationModel.ActivateUseCase(newProjectUseCase);
        }
    }
}
