using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyle.Infrastructure.ViewToRegionBinding;
using Microsoft.Practices.Composite.Regions;
using PTWpf.Modules.ModuleEvents;

namespace PTWpf.Modules.Resource
{
    /// <summary>
    /// Use case for all E-mail functionality in the main window like showing the inbox, reading emails in the preview window, etc..
    /// </summary>
    public class NewResourceUseCase : ActiveAwareUseCaseController
    {
        // The references to these viewmodels will be filled when the app is initialized for the first time. 
        private ResourceEditViewModel _resourceEditViewModel;

        public NewResourceUseCase(
            // Get the ViewToRegionBinder that the baseclass needs
            IViewToRegionBinder viewtoToRegionBinder
            // Get the factories that can create the viewmodels
            , ObjectFactory<ResourceEditViewModel> resourceEditViewModel
            , IModelVisualizationRegistry modelVisualizationRegistry)
           : base(viewtoToRegionBinder)
        {
            // Just before the view is initialized for the first time
            this.AddInitializationMethods(
                // Create the emailViewModel and assign it to this variable
               () => this._resourceEditViewModel = resourceEditViewModel.CreateInstance());

            // Register visualizations for these view models. This means: whenever a viewmodel is displayed, 
            // use this type of view to visualize it. 
            modelVisualizationRegistry.Register<ResourceEditViewModel, ResourceEditView>();
        }

        public override string Name
        {
            // UseCases with a Name of length=0 will be not visible (collapsed) in the outlook button bar...
            get { return ""; }
        }

        public int ResourceId { get; set; }

        protected override void OnFirstActivation()
        {
            // Make sure the Toolbar and the Mainregion get displayed when this use case is activated
            // and removed again when it becomes inactive. 
            this.ViewToRegionBinder.Add("MainRegion", this._resourceEditViewModel);
            this._resourceEditViewModel.RegionContext.Value = this.ResourceId;
        }


    }
}
