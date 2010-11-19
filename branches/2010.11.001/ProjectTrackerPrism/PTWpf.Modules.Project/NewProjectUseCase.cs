using System;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using Microsoft.Practices.Composite.Regions;
using PTWpf.Modules.ModuleEvents;
using PTWpf.Project.Modules;
using Microsoft.Practices.Composite.Events;

namespace PTWpf.Modules.Project
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class NewProjectUseCase : ActiveAwareUseCaseController
    {
        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private ProjectEditViewModel _projectEditViewModel;

        public NewProjectUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            // Get the factories that can create the viewmodels
            , ObjectFactory<ProjectEditViewModel> projectEditViewModel
            , IApplicationModel applicationModel
            , IModelVisualizationRegistry modelVisualizationRegistry)
            : base(viewtoToRegionBinder)
        {
            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
               () => this._projectEditViewModel = projectEditViewModel.CreateInstance());

            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            modelVisualizationRegistry.Register<ProjectEditViewModel, ProjectEditView>();
        }

        public override string Name
        {
            get { return ""; }
        }
        public Guid ProjectId { get; set; }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("MainRegion", this._projectEditViewModel);
            this._projectEditViewModel.RegionContext.Value =this.ProjectId;
        }
    }
}
