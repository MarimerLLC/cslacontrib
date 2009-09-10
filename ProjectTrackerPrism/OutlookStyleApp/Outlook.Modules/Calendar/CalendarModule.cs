using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using OutlookStyle.Infrastructure.ApplicationModel;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace Outlook.Modules.Calendar
{
    public class CalendarModule : IModule
    {
        private readonly IApplicationModel applicationModel;
        private readonly CalendarMainUseCase mainCalenderUseCase;
        private readonly IRegionManager regionManager;
        private readonly IModelVisualizationRegistry modelVisualizationRegistry;

        public CalendarModule(IApplicationModel applicationModel
            , CalendarMainUseCase mainCalenderUseCase
            , IRegionManager regionManager
            , IModelVisualizationRegistry modelVisualizationRegistry)
        {
            this.applicationModel = applicationModel;
            this.mainCalenderUseCase = mainCalenderUseCase;
            this.regionManager = regionManager;
            this.modelVisualizationRegistry = modelVisualizationRegistry;
        }

        public void Initialize()
        {
            // Add the calendar Main usecase, which will add a button to the buttonbar. Clicking on the button will activate the usecase
            applicationModel.AddMainUseCase(mainCalenderUseCase);

            // Register a visualization for the CalendarMainViewMOdel. Whenever a region shows the CalenderMainViewModel, use the CalendarMainView to visualize it. 
            // This is done by the VisualizingRegionManagerRegistrationBehavior which is added to all regions by default. 
            modelVisualizationRegistry.Register<CalendarMainViewModel, CalendarMainView>();

            // Whenever the GadgetRegion gets displayed, add a Gadget to it. This displays a different (More direct) way of visualizing ViewModels.
            // The ViewViewModelWrapper wraps both a View and a ViewModel. It is a control, so it can be placed in the visual tree (and thus easily
            // be added to a region)
            regionManager.RegisterViewWithRegion("GatgetRegion", typeof(ViewViewModelWrapper<CalendarSidebarGadget, CalendarSideBarGadgetViewModel>));
        }
    }
}
