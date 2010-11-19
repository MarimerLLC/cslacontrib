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
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Resource
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class ResourceUseCase : ActiveAwareUseCaseController
    {
        private IApplicationModel ApplicationModel { get; set; }
        private IEventAggregator EventAggregator { get; set; }
        private IUnityContainer Container { get; set; }
        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private ResourceListViewModel _resourceListViewModel;
        private ResourceToolbarViewModel _resourceToolbarViewModel;

        public ResourceUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            , IRegionManager regionManager
            , IUnityContainer container
            , IEventAggregator eventAggregator
            // Get the factories that can create the viewmodels
            , ObjectFactory<ResourceListViewModel> resourceViewModel
            , ObjectFactory<ResourceToolbarViewModel> resourceToolbarViewModel
            , IApplicationModel applicationModel
            , IModelVisualizationRegistry modelVisualizationRegistry)
           : base(viewtoToRegionBinder)
        {
            this.ApplicationModel = applicationModel;
            this.Container = container;
            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
               () => this._resourceListViewModel = resourceViewModel.CreateInstance()
               , () => this._resourceToolbarViewModel = resourceToolbarViewModel.CreateInstance());

            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            modelVisualizationRegistry.Register<ResourceListViewModel, ResourceListView>();
            modelVisualizationRegistry.Register<ResourceToolbarViewModel, ResourceToolbarView>();
            modelVisualizationRegistry.Register<ResourceEditViewModel, ResourceEditView>();

            container.RegisterType<IResourceAssignService, ResourceAssignmentService>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(container.Resolve<IResourceAssignService>());

            regionManager.RegisterViewWithRegion("ResourceEditRegion", typeof(ResourceEditViewModel));
            // watch for OpenResourceEvents fired by (Link)Button command in ProjectEditViewModel...
            eventAggregator.GetEvent<OpenResourceByIdEvent>().Subscribe(OpenResourceById);
        }

        public override string Name
        {
            get { return "Resources"; }
        }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("MainRegion", this._resourceListViewModel);
            this.ViewToRegionBinder.Add("ToolbarRegion", this._resourceToolbarViewModel);
        }

        void OpenResourceById(int resourceId)
        {
            NewResourceUseCase newResourceUseCase = this.Container.Resolve<NewResourceUseCase>();
            newResourceUseCase.ResourceId = resourceId;
            ApplicationModel.AddMainUseCase(newResourceUseCase);
            ApplicationModel.ActivateUseCase(newResourceUseCase);
        }
    }
}
